using Contexts;
using Entities.Common;
using Entities.DAOs;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;
using Services.Implementation.Extensions;

namespace Services.Implementation
{
    public class TimetableService(HsmsDbContext context) : ITimetableService
    {
        private readonly HsmsDbContext _context = context;

        private const int STEPCOUNT = 10000;

        public Timetable Create(TimetableParameters parameters)
        {
            var (classes, teachers, subjects, assignments, timetableFlags) = _context.GetData(parameters);

            // Tạo cá thể gốc
            /* Cá thể gốc là cá thể được tạo ra đầu tiên và là cá thể mang giá trị mặc định và bất biến*/
            var root = TimetableCreator.CreateRootIndividual(classes, teachers, assignments, timetableFlags, parameters);

            // Tạo quần thể ban đầu và tính toán độ thích nghi
            var timetablePopulation = new List<TimetableIndividual>();
            for (var count = 0; count < 100; count++)
            {
                var individual = root.Clone();
                individual.RandomlyAssign();
                individual.CalculateAdaptability(parameters);
                timetablePopulation.Add(individual);
            }
            // timetablePopulation = [.. timetablePopulation.OrderBy(i => i.Adaptability)];

            for (var step = 1; step <= STEPCOUNT; step++)
            {
                if (timetablePopulation.Min(i => i.Adaptability) < 1000)
                    break;

                timetablePopulation.ForEach(i => i.Age += 1);

                // Lai tạo
                /*
                 * Tiến hành xáo trộn ngẫu nhiên quần thể
                 * Lấy 1/2 số lượng cá thể trong quần thể ngẫu nhiên và cho chúng lai ghép với nhau
                 * parent1 x parent2 --> child1, child2
                 */
                var randIndexes = Enumerable.Range(0, timetablePopulation.Count).Shuffle().ToList();
                var timetableChildren = new List<TimetableIndividual>();
                for (var k = 0; k < randIndexes.Count / 2 - 1; k += 2)
                {
                    var parent1 = timetablePopulation[randIndexes[k]];
                    var parent2 = timetablePopulation[randIndexes[k + 1]];

                    var children = root.Crossover(parent1, parent2, parameters, (int)ETimetableCreator.CrossoverUsingClassChromosome);
                    timetableChildren.AddRange(children);
                }

                // Chọn lọc
                /* Cá thể con thay thế các cá thể bố mẹ nếu bố mẹ có độ thích nghi thấp */
                for (var k = 0; k < randIndexes.Count / 2 - 1; k += 2)
                {
                    var parent1 = timetablePopulation[randIndexes[k]];
                    var parent2 = timetablePopulation[randIndexes[k + 1]];

                    var child1 = timetableChildren[k];
                    var child2 = timetableChildren[k + 1];

                    var list = new List<TimetableIndividual>() { parent1, parent2, child1, child2 }.OrderBy(i => i.Adaptability).ToList();

                    timetablePopulation.Remove(parent1);
                    timetablePopulation.Remove(parent2);
                    timetablePopulation.Add(list[0]);
                    timetablePopulation.Add(list[1]);
                }

                /* Loại bỏ các cá thể già yếu */
                // timetablePopulation = [.. timetablePopulation.Where(i => i.Age < i.Longevity)];

                //timetablePopulation.InsertRange(0, timetableChildren);
                //timetablePopulation = [.. timetablePopulation.Take(100)];

                Console.WriteLine($"step {step}, best score {timetablePopulation.Min(i => i.Adaptability)}, population {timetablePopulation.Count}");
            }

            timetablePopulation.OrderBy(i => i.Adaptability).First().ToCsv();

            throw new NotImplementedException();
        }

        public Timetable CreateDemo(TimetableParameters parameters)
        {
            // Khởi tạo các biến
            var classes = new List<ClassTCDTO>();
            var teachers = new List<TeacherTCDTO>();
            var subjects = new List<SubjectTCDTO>();
            var assignments = new List<AssignmentTCDTO>();
            var timetableUnits = new List<TimetableUnitTCDTO>();
            ETimetableFlag[,] timetableFlag = null!;
            var rand = new Random();

            // Lấy dữ liệu từ database
            {
                var classesDb = _context.Classes
                    .Where(c => parameters.ClassIds.Contains(c.Id) &&
                                c.StartYear == parameters.StartYear &&
                                c.EndYear == parameters.EndYear)
                    .Include(c => c.SubjectClasses)
                        .ThenInclude(sc => sc.Subject)
                    .OrderBy(c => c.Name)
                    .AsNoTracking()
                    .ToList()
                    ?? throw new Exception();
                foreach (var @class in classesDb)
                    classes.Add(new ClassTCDTO(@class));
                if (classes.Count != parameters.ClassIds.Count)
                    throw new Exception();

                timetableFlag = new ETimetableFlag[classes.Count, 61];

                var subjectsDb = _context.Subjects.AsNoTracking().ToList();
                foreach (var subject in subjectsDb)
                    subjects.Add(new SubjectTCDTO(subject));

                var assignmentsDb = _context.Assignments
                    .Where(a => a.StartYear == parameters.StartYear &&
                                a.EndYear == parameters.EndYear &&
                                a.Semester == parameters.Semester)
                    .AsNoTracking()
                    .ToList()
                    ?? throw new Exception();

                var teacherIds = assignmentsDb.Select(a => a.TeacherId).Distinct().ToList();
                var teachersDb = _context.Teachers
                    .Where(t => teacherIds.Contains(t.Id))
                    .OrderBy(c => c.LastName)
                    .AsNoTracking()
                    .ToList()
                    ?? throw new Exception();
                foreach (var teacher in teachersDb)
                    teachers.Add(new TeacherTCDTO(teacher));

                foreach (var assignment in assignmentsDb)
                {
                    var teacher = teachers.First(t => t.Id == assignment.TeacherId);
                    var @class = classes.First(c => c.Id == assignment.ClassId);
                    var subject = subjects.First(s => s.Id == assignment.SubjectId);
                    assignments.Add(new AssignmentTCDTO(assignment, teacher, @class, subject));
                }

                // Kiểm tra xem tất cả các lớp đã được phân công đầy đủ hay chưa
                foreach (var c in classesDb)
                {
                    var periodCount = 0;
                    foreach (var sc in c.SubjectClasses)
                    {
                        var assignment = assignmentsDb.FirstOrDefault(a => a.SubjectId == sc.SubjectId && a.ClassId == sc.ClassId)
                            ?? throw new Exception($"Class: {c.Name}, Subject: {subjects.First(s => s.Id == sc.SubjectId).ShortName}");
                        if (assignment.PeriodCount != sc.PeriodCount || assignment.TeacherId == Guid.Empty)
                            throw new Exception();
                        periodCount += sc.PeriodCount;
                    }
                    if (periodCount != c.PeriodCount) throw new Exception();
                }
            }

            // Tạo các timetableUnit ứng với mỗi Assignment và thêm vào tkb
            /* Đánh dấu vị trí các các tiết trống */
            for (var i = 0; i < classes.Count; i++)
            {
                var a = classes[i].SchoolShift == ESchoolShift.Morning ? 0 : 5;
                for (var j = 1; j < 61; j += 10)
                    for (var k = j; k < j + 5; k++)
                        timetableFlag[i, k + a] = ETimetableFlag.Unfilled;
                foreach (var unit in parameters.FreeTimetableUnits.Where(u => u.ClassName == classes[i].Name))
                    timetableFlag[i, unit.StartAt] = ETimetableFlag.None;
            }

            /* Thêm các tiết được xếp sẵn trước */
            foreach (var unit in parameters.FixedTimetableUnits)
            {
                var newUnit = new TimetableUnitTCDTO(assignments.First(a => a.Id == unit.AssignmentId))
                {
                    StartAt = unit.StartAt,
                    Priority = unit.Priority,
                };
                timetableUnits.Add(newUnit);
            }

            /* Đánh dấu các tiết này vào timetableFlag */
            foreach (var u in timetableUnits)
            {
                var classIndex = classes.IndexOf(classes.First(c => c.Name == u.ClassName));
                var startAt = u.StartAt;
                timetableFlag[classIndex, startAt] = ETimetableFlag.Fixed;
            }

            timetableFlag.ToCsv(classes);

            /* Thêm các tiết chưa được xếp vào sau */
            foreach (var assignment in assignments)
            {
                var count = parameters.FixedTimetableUnits.Count(u => u.AssignmentId == assignment.Id);
                for (var i = 0; i < assignment.PeriodCount - count; i++)
                    timetableUnits.Add(new TimetableUnitTCDTO(assignment));
            }
            timetableUnits = [.. timetableUnits.OrderBy(u => u.ClassName)];

            // Tạo cá thể tkb gốc
            /* 
             * Các tiết (timetableUnit) có Priority = None trong cá thể tkb này chưa được xếp 
             * và các cá thể thuộc thế hệ F0 sẽ được clone từ cá thể này
             */
            var timetableRoot = new TimetableIndividual(timetableFlag, timetableUnits, classes, teachers);
            timetableRoot.RandomlyAssign();

            var timetable = new Timetable()
            {
                Name = "TKB Demo",
                StartYear = 2023,
                EndYear = 2024,
                Semester = 1,
            };

            return timetable;
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Timetable Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Timetable timetable)
        {
            throw new NotImplementedException();
        }


    }
}
