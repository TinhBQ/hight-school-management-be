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
                foreach (var unit in parameters.FreeTimetableUnits.Where(u => u.Assignment.Class.Id == classes[i].Id))
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
                var classIndex = classes.IndexOf(classes.First(c => c.Name == u.Assignment.Class.Name));
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
            timetableUnits = [.. timetableUnits.OrderBy(u => u.Assignment.Class.Name)];

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

        public Timetable Create(TimetableParameters parameters)
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
                foreach (var unit in parameters.FreeTimetableUnits.Where(u => u.Assignment.Class.Id == classes[i].Id))
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
                var classIndex = classes.IndexOf(classes.First(c => c.Name == u.Assignment.Class.Name));
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
            timetableUnits = [.. timetableUnits.OrderBy(u => u.Assignment.Class.Name)];

            // Tạo cá thể tkb gốc
            /* 
             * Các tiết (timetableUnit) có Priority = None trong cá thể tkb này chưa được xếp 
             * và các cá thể thuộc thế hệ F0 sẽ được clone từ cá thể này
             */
            var timetableRoot = new TimetableIndividual(timetableFlag, timetableUnits, classes, teachers);

            // Tạo quần thể TKB và tính toán độ thích nghi
            var timetablePopulation = new List<TimetableIndividual>();
            for (var count = 0; count < 1000; count++)
            {
                var individual = timetableRoot.Clone();
                individual.RandomlyAssign();
                individual.UpdateAdaptability(parameters);
                timetablePopulation.Add(individual);
            }
            timetablePopulation = [.. timetablePopulation.OrderBy(i => i.Adaptability).Take(1000)];

            //var stepCount = 10;
            //for (var step = 1; step <= stepCount; step++)
            //{
            //    if (timetablePopulation.First().Adaptability == 0)
            //        break;
            //    // Lai tạo
            //    var rand = new Random();
            //    for (var z = 0; z < 1000; z++)
            //    {
            //        var index = rand.Next(0, 1000);
            //        var parent1 = timetablePopulation[rand.Next(0, index)];
            //        var parent2 = timetablePopulation[rand.Next(index, 1000)];

            //        var children = timetableRoot.Crossover(parent1, parent2, parameters);
            //        timetablePopulation.AddRange(children);
            //    }

            //    // Chọn lọc
            //    timetablePopulation = [.. timetablePopulation.OrderBy(i => i.Adaptability).Take(1000)];
            //    Console.WriteLine($"step {step}, best score {timetablePopulation.First().Adaptability}");
            //}

            //var result = timetablePopulation.First();

            throw new NotImplementedException();
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
