using Contexts;
using Entities.DAOs;
using Services.Abstraction.IApplicationServices;

namespace Services.Implementation
{
    public class TimetableService(HsmsDbContext context) : ITimetableService
    {
        private readonly HsmsDbContext _context = context;

        public Timetable Create(
            List<Guid> classIds,
            List<string> doublePeriodSubjectShortNames,
            List<TimetableUnit> fixedTimetableUnits,
            List<TimetableUnit> busyUnits,
            int maxEmptyPeriodCount,
            List<(Guid, int, int)> maxMinPeriodCount,
            bool?[,] timetableFlag,
            int startYear,
            int endYear,
            int semester)
        {
            //// Khởi tạo các biến

            ///* Khởi tạo danh sách các lớp học được chọn để xếp tkb */
            //var classesFromDb = _context.Classes
            //    .Where(c => classIds.Contains(c.Id) && c.StartYear == startYear && c.EndYear == endYear)
            //    .Include(c => c.SubjectClasses)
            //    .Include(c => c.Assignments.Where(a => a.Semester == semester))
            //    .ThenInclude(a => a.Teacher)
            //    .OrderBy(c => c.Name)
            //    .AsNoTracking()
            //    .ToList();

            ///* Khởi tạo danh sách các phân công của các lớp */
            //var assignmentsFromDb = new List<Assignment>();
            //classesFromDb.ForEach(c => assignmentsFromDb.AddRange(c.Assignments));

            ///* Khởi tạo danh sách giáo viên được phân công */
            //var teachersFromDb = new List<Teacher>();
            //assignmentsFromDb.ForEach(a =>
            //{ if (!teachersFromDb.Select(t => t.Id).Contains(a.TeacherId)) teachersFromDb.Add(a.Teacher); });

            ///* Khởi tạo danh sách môn học */
            //var subjects = _context.Subjects.AsNoTracking().ToList();

            ///* Khởi tạo danh sách thời khóa biểu của lớp và giáo viên */
            //var classTimetables = new List<ClassTimetable>();
            //var teacherTimetables = new List<TeacherTimetable>();

            ///* Xếp các phân công vào lớp và giáo viên tương ứng */
            //teachersFromDb.ForEach(teacher =>
            //{
            //    teacherTimetables.Add(new TeacherTimetable { Teacher = teacher });
            //    teacher.Assignments = [.. assignmentsFromDb.Where(assignment => assignment.TeacherId == teacher.Id)];
            //});
            //classesFromDb.ForEach(@class =>
            //{
            //    classTimetables.Add(new ClassTimetable { Class = @class });
            //    @class.Assignments = [.. assignmentsFromDb.Where(assignment => assignment.ClassId == @class.Id)];
            //});

            //// Kiểm tra xem tất cả các lớp đã được phân công đầy đủ hay chưa

            //foreach (var c in classesFromDb)
            //{
            //    foreach (var sc in c.SubjectClasses)
            //    {
            //        var assignment = assignmentsFromDb.FirstOrDefault(a => a.SubjectId == sc.SubjectId && a.ClassId == sc.ClassId)
            //            ?? throw new Exception($"Class: {c.Name}, Subject: {sc.SubjectId}");
            //        if (assignment.PeriodCount != sc.PeriodCount || assignment.TeacherId == null)
            //            throw new Exception();
            //    }
            //}

            //// Tạo các timetableUnit ứng với mỗi Assignment và thêm vào tkb

            //var timetableUnits = new List<TimetableUnit>();

            ///* Thêm các tiết được xếp sẵn trước */
            //timetableUnits.AddRange(fixedTimetableUnits);

            ///* Đánh dấu các tiết này vào timetableFlag */
            //foreach (var u in timetableUnits)
            //{
            //    var classIndex = classesFromDb.IndexOf(classesFromDb.First(c => c.Name == u.ClassName));
            //    var startAt = u.StartAt;
            //    timetableFlag[classIndex, startAt] = null;
            //}

            ///* Thêm các tiết chưa được xếp vào sau */
            //foreach (var assignment in assignmentsFromDb)
            //{
            //    var count = fixedTimetableUnits.Count(u =>
            //        u.Assignment.TeacherId == assignment.TeacherId &&
            //        u.Assignment.ClassId == assignment.ClassId &&
            //        u.Assignment.SubjectId == assignment.SubjectId);
            //    for (var i = 0; i < assignment.PeriodCount - count; i++)
            //    {
            //        var unit = new TimetableUnit()
            //        {
            //            /*Id = Nanoid.Generate("abcdefghijklmnopqrstuvwxyz", 10),*/
            //            Id = Guid.NewGuid(),
            //            AssignmentId = assignment.Id,
            //            Assignment = assignment,
            //            SubjectName = subjects.First(s => s.Id == assignment.SubjectId).ShortName,
            //            ClassName = classesFromDb.First(c => c.Id == assignment.ClassId).Name,
            //            TeacherName = teachersFromDb.First(t => t.Id == assignment.TeacherId).ShortName,
            //            Priority = EPriority.None
            //        };
            //        timetableUnits.Add(unit);
            //    }
            //}
            //timetableUnits = [.. timetableUnits.OrderBy(u => u.ClassName)];

            //// Thêm các tiết học vào thời khóa biểu của các lớp và các giáo viên
            //teacherTimetables.ForEach(teacherTimetable =>
            //{
            //    teacherTimetable.Periods = [.. timetableUnits.Where(p => p.Assignment.TeacherId == teacherTimetable.Teacher.Id)];
            //});
            //classTimetables.ForEach(classTimetable =>
            //{
            //    classTimetable.Periods = [.. timetableUnits.Where(p => p.Assignment.ClassId == classTimetable.Class.Id)];
            //});
            //classTimetables = [.. classTimetables.OrderBy(c => c.Class.Name)];

            //// Tạo cá thể tkb gốc
            ///* 
            // * Các tiết (timetableUnit) trong cá thể tkb này chưa được xếp 
            // * và các cá thể thuộc thế hệ F0 sẽ được clone từ cá thể này
            // */
            //var timetableRoot = new TimetableIndividual(
            //    timetableFlag, classTimetables, teacherTimetables, timetableUnits, 0);

            //// Tạo quần thể TKB và tính toán độ thích nghi
            //var timetablePopulation = new List<TimetableIndividual>();
            //for (var count = 0; count < 10000; count++)
            //{
            //    var individual = timetableRoot.Clone();
            //    individual.ArrangesRandomly();
            //    individual.CalculateAdaptability(fixedTimetableUnits, doublePeriodSubjectShortNames, busyUnits, maxEmptyPeriodCount, maxMinPeriodCount);
            //    timetablePopulation.Add(individual);
            //}

            //// Chọn lọc
            //timetablePopulation = timetablePopulation.OrderBy(i => i.Adaptability).Take(1000).ToList();

            //var newP = new List<TimetableIndividual>();
            //// Lai tạo
            //var rand = new Random();
            //for (var z = 0; z < 1000; z++)
            //{
            //    var parent1 = timetablePopulation[rand.Next(0, 1000)];
            //    var parent2 = timetablePopulation[rand.Next(0, 1000)];

            //    var teacherTimetable1 = parent1.TeacherTimetables[rand.Next(0, parent1.TeacherTimetables.Count)];
            //    var teacherTimetable2 = parent2.TeacherTimetables.First(t => t.Teacher.Id == teacherTimetable1.Teacher.Id);

            //    teacherTimetable1.Periods = [.. teacherTimetable1.Periods.OrderBy(p => p.ClassName)];
            //    teacherTimetable2.Periods = [.. teacherTimetable2.Periods.OrderBy(p => p.ClassName)];

            //    var periodCount = teacherTimetable1.Periods.Count;
            //    var a = periodCount - rand.Next(1, periodCount - 1);

            //    var newPeriods = teacherTimetable1.Periods.Take(a).ToList();
            //    newPeriods.AddRange(teacherTimetable2.Periods.Skip(a).ToList());

            //    var newIndividual = timetableRoot.Clone();
            //    foreach (var period in newIndividual.Periods.Where(p => newPeriods.Select(np => np.Id).Contains(p.Id)))
            //    {
            //        period.StartAt = newPeriods.First(p => p.Id == period.Id).StartAt;
            //    }
            //    for (var i = 0; i < newIndividual.ClassTimetables.Count; i++)
            //        foreach (var period in newIndividual.ClassTimetables[i].Periods)
            //            newIndividual.TimetableFlag[i, period.StartAt] = false;
            //    newIndividual.CalculateAdaptability(fixedTimetableUnits, doublePeriodSubjectShortNames, busyUnits, maxEmptyPeriodCount, maxMinPeriodCount);
            //    newP.Add(newIndividual);

            //    //var timetableUnit1 = teacherTimetable1.Periods[rand.Next(0, teacherTimetable1.Periods.Count)];
            //    //var timetableUnit2 = teacherTimetable2.Periods.First(p => p.Id == timetableUnit1.Id);

            //    //(timetableUnit1.StartAt, timetableUnit2.StartAt) = (timetableUnit2.StartAt, timetableUnit1.StartAt);
            //}


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
