using Contexts;
using Entities.Common;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementation.Extensions
{
    internal static class TimetableCreator
    {
        private static readonly Random rand = new();

        internal static (
            List<ClassTCDTO>,
            List<TeacherTCDTO>,
            List<SubjectTCDTO>,
            List<AssignmentTCDTO>,
            ETimetableFlag[,])
            GetData(this HsmsDbContext _context, TimetableParameters parameters)
        {
            // Khởi tạo các biến
            var classes = new List<ClassTCDTO>();
            var teachers = new List<TeacherTCDTO>();
            var subjects = new List<SubjectTCDTO>();
            var assignments = new List<AssignmentTCDTO>();
            var timetableUnits = new List<TimetableUnitTCDTO>();
            ETimetableFlag[,] timetableFlags = null!;

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
            for (var i = 0; i < classesDb.Count; i++)
                classes.Add(new ClassTCDTO(classesDb[i]));
            if (classes.Count != parameters.ClassIds.Count)
                throw new Exception();

            timetableFlags = new ETimetableFlag[classes.Count, 61];

            var subjectsDb = _context.Subjects.AsNoTracking().ToList();
            for (var i = 0; i < subjectsDb.Count; i++)
                subjects.Add(new SubjectTCDTO(subjectsDb[i]));

            var assignmentsDb = _context.Assignments
                .Where(a => a.StartYear == parameters.StartYear &&
                            a.EndYear == parameters.EndYear &&
                            a.Semester == parameters.Semester &&
                            classesDb.Select(c => c.Id).Contains(a.ClassId))
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
            for (var i = 0; i < teachersDb.Count; i++)
                teachers.Add(new TeacherTCDTO(teachersDb[i]));

            for (var i = 0; i < assignmentsDb.Count; i++)
            {
                var teacher = teachers.First(t => t.Id == assignmentsDb[i].TeacherId);
                var @class = classes.First(c => c.Id == assignmentsDb[i].ClassId);
                var subject = subjects.First(s => s.Id == assignmentsDb[i].SubjectId);
                assignments.Add(new AssignmentTCDTO(assignmentsDb[i], teacher, @class, subject));
            }

            // Kiểm tra xem tất cả các lớp đã được phân công đầy đủ hay chưa
            for (var i = 0; i < classesDb.Count; i++)
            {
                var periodCount = 0;
                for (var j = 0; j < classesDb[i].SubjectClasses.Count; j++)
                {
                    var subjectClass = classesDb[i].SubjectClasses.ToList()[j];
                    var assignment = assignmentsDb.FirstOrDefault(a => a.SubjectId == subjectClass.SubjectId && a.ClassId == subjectClass.ClassId)
                        ?? throw new Exception($"Class: {classesDb[i].Name}, Subject: {subjects.First(s => s.Id == subjectClass.SubjectId).ShortName}");
                    if (assignment.PeriodCount != subjectClass.PeriodCount || assignment.TeacherId == Guid.Empty)
                        throw new Exception();
                    periodCount += subjectClass.PeriodCount;
                }
                if (periodCount != classesDb[i].PeriodCount) throw new Exception();
            }

            return (classes, teachers, subjects, assignments, timetableFlags);
        }

        internal static TimetableRootIndividual CreateRootIndividual(
            List<ClassTCDTO> classes,
            List<TeacherTCDTO> teachers,
            List<AssignmentTCDTO> assignments,
            ETimetableFlag[,] timetableFlags,
            TimetableParameters parameters)
        {
            // Tạo các timetableUnit ứng với mỗi Assignment và thêm vào tkb

            /* Đánh dấu vị trí các các tiết trống */
            for (var i = 0; i < classes.Count; i++)
            {
                var a = classes[i].SchoolShift == ESchoolShift.Morning ? 0 : 5;
                for (var j = 1; j < 61; j += 10)
                    for (var k = j; k < j + 5; k++)
                        timetableFlags[i, k + a] = ETimetableFlag.Unfilled;

                var list = parameters.FreeTimetableUnits.Where(u => u.ClassName == classes[i].Name).ToList();
                for (var j = 0; j < list.Count; j++)
                    timetableFlags[i, list[j].StartAt] = ETimetableFlag.None;
            }

            /* Tạo danh sách timetableUnit và thêm các tiết được xếp sẵn trước*/
            var timetableUnits = new List<TimetableUnitTCDTO>();

            for (var i = 0; i < parameters.FixedTimetableUnits.Count; i++)
            {
                var newUnit = new TimetableUnitTCDTO(assignments.First(a => a.Id == parameters.FixedTimetableUnits[i].AssignmentId))
                {
                    Id = parameters.FixedTimetableUnits[i].Id,
                    StartAt = parameters.FixedTimetableUnits[i].StartAt,
                    Priority = parameters.FixedTimetableUnits[i].Priority,
                };
                timetableUnits.Add(newUnit);
            }

            /* Đánh dấu các tiết này vào timetableFlags */
            for (var i = 0; i < timetableUnits.Count; i++)
            {
                var classIndex = classes.IndexOf(classes.First(c => c.Name == timetableUnits[i].ClassName));
                var startAt = timetableUnits[i].StartAt;
                timetableFlags[classIndex, startAt] = ETimetableFlag.Fixed;
            }

            /* Thêm các tiết chưa được xếp vào sau */
            for (var i = 0; i < assignments.Count; i++)
            {
                var count = parameters.FixedTimetableUnits.Count(u => u.AssignmentId == assignments[i].Id);
                for (var j = 0; j < assignments[i].PeriodCount - count; j++)
                    timetableUnits.Add(new TimetableUnitTCDTO(assignments[i]));
            }

            timetableUnits = [.. timetableUnits.OrderBy(u => u.ClassName)];

            return new TimetableRootIndividual(timetableFlags, timetableUnits, classes, teachers);
        }

        internal static TimetableIndividual Clone(this TimetableRootIndividual src)
        {
            var classCount = src.TimetableFlag.GetLength(0);
            var periodCount = src.TimetableFlag.GetLength(1);
            var timetableFlag = new ETimetableFlag[classCount, periodCount];
            for (var i = 0; i < classCount; i++)
                for (var j = 0; j < periodCount; j++)
                    timetableFlag[i, j] = src.TimetableFlag[i, j];

            var timetableUnits = new List<TimetableUnitTCDTO>();
            for (var i = 0; i < src.TimetableUnits.Count; i++)
                timetableUnits.Add(src.TimetableUnits[i] with { });
            return new TimetableIndividual(timetableFlag, timetableUnits, src.Classes, src.Teachers) { Age = 1, Longevity = rand.Next(30, 100) };
        }

        internal static void RandomlyAssign(this TimetableIndividual src)
        {
            for (var i = 0; i < src.TimetableFlag.GetLength(0); i++)
            {
                var startAts = new List<int>();
                for (var j = 1; j < src.TimetableFlag.GetLength(1); j++)
                    if (src.TimetableFlag[i, j] == ETimetableFlag.Unfilled)
                        startAts.Add(j);
                var timetableUnits = src.TimetableUnits
                    .Where(u => u.ClassName == src.Classes[i].Name && u.StartAt == 0)
                    .OrderBy(u => new Random().Next())
                    .ToList();
                if (startAts.Count != timetableUnits.Count) throw new Exception();
                for (var j = 0; j < timetableUnits.Count; j++)
                    timetableUnits[j].StartAt = startAts[j];
            }
        }

        #region Crossover Methods
        internal static List<TimetableIndividual> Crossover(
            this TimetableRootIndividual root,
            TimetableIndividual parent1,
            TimetableIndividual parent2,
            TimetableParameters parameters,
            int method)
        {
            return method switch
            {
                (int)ETimetableCreator.CrossoverUsingClassChromosome
                    => CrossoverUsingClassChromosome(root, parent1, parent2, parameters),
                _ => throw new NotImplementedException(),
            };
        }

        private static List<TimetableIndividual> CrossoverUsingClassChromosome(
            TimetableRootIndividual root, TimetableIndividual parent1, TimetableIndividual parent2, TimetableParameters parameters)
        {
            var child1 = root.Clone();
            var child2 = root.Clone();

            var randClassCount = rand.Next(1, root.Classes.Count / 2);
            var randClassNames = root.Classes.Select(c => c.Name).Shuffle().Take(randClassCount).ToList();
            var randClassName = root.Classes[rand.Next(1, root.Classes.Count - 1)].Name;

            for (var i = 0; i < root.TimetableUnits.Count; i++)
            {
                child1.TimetableUnits[i].StartAt = randClassNames.Contains(root.TimetableUnits[i].ClassName)
                    ? parent1.TimetableUnits[i].StartAt : parent2.TimetableUnits[i].StartAt;
                child2.TimetableUnits[i].StartAt = !randClassNames.Contains(root.TimetableUnits[i].ClassName)
                    ? parent1.TimetableUnits[i].StartAt : parent2.TimetableUnits[i].StartAt;
            }

            // Đột biến
            if (rand.Next(0, 100) < 50)
            {
                var ClassName = root.Classes[rand.Next(0, root.Classes.Count)].Name;
                var timetableUnits = child1.TimetableUnits
                    .Where(u => u.ClassName == ClassName && u.Priority != EPriority.Fixed).ToList();
                var randNumList = Enumerable.Range(0, timetableUnits.Count).Shuffle().ToList();
                for (var i = 0; i < randNumList.Count - rand.Next(0, randNumList.Count - 1); i++)
                    Swap(timetableUnits[randNumList[i]], timetableUnits[randNumList[i + 1]]);
            }
            if (rand.Next(0, 100) < 50)
            {
                var ClassName = root.Classes[rand.Next(0, root.Classes.Count)].Name;
                var timetableUnits = child2.TimetableUnits
                    .Where(u => u.ClassName == ClassName && u.Priority != EPriority.Fixed).ToList();
                var randNumList = Enumerable.Range(0, timetableUnits.Count).Shuffle().ToList();
                for (var i = 0; i < randNumList.Count - rand.Next(0, randNumList.Count - 1); i++)
                    Swap(timetableUnits[randNumList[i]], timetableUnits[randNumList[i + 1]]);
            }

            child1.CalculateAdaptability(parameters);
            child2.CalculateAdaptability(parameters);

            return [child1, child2];
        }

        private static void Swap(TimetableUnitTCDTO a, TimetableUnitTCDTO b)
        {
            (a.StartAt, b.StartAt) = (b.StartAt, a.StartAt);
        }
        #endregion

        #region Fitness Function
        internal static void CalculateAdaptability(this TimetableIndividual src, TimetableParameters parameters)
        {
            src.Adaptability = (
                  src.CheckH01()
                + src.CheckH02()
                + src.CheckH03(parameters)
                + src.CheckH05()
                + src.CheckH06(parameters)
                + src.CheckH09(parameters)
                + src.CheckH11()
                ) * 1000;
        }

        private static int CheckH01(this TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Classes.Count; i++)
            {
                var timetableUnits = src.TimetableUnits.Where(u => u.ClassName == src.Classes[i].Name).ToList();
                var uniqueUnitCount = timetableUnits.Select(u => u.StartAt).Distinct().Count();
                count += (timetableUnits.Count - uniqueUnitCount);
            }
            return count * 2;
        }

        private static int CheckH02(this TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.TimetableUnits.Count; i++)
                if (src.TimetableUnits[i].StartAt == 0)
                    count++;
            return count;
        }

        private static int CheckH03(this TimetableIndividual src, TimetableParameters parameters)
        {
            var count = 0;
            var fixedUnits = src.TimetableUnits.Where(u => u.Priority == EPriority.Fixed).ToList();
            if (fixedUnits.Count != parameters.FixedTimetableUnits.Count) throw new Exception();
            for (var i = 0; i < fixedUnits.Count; i++)
                if (!parameters.FixedTimetableUnits
                    .Any(u => u.AssignmentId == fixedUnits[i].AssignmentId && u.StartAt == fixedUnits[i].StartAt))
                    count++;
            return count;
        }

        private static void CheckH04(this TimetableIndividual src) { }

        private static int CheckH05(this TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var timetableUnits = src.TimetableUnits.Where(u => u.TeacherName == src.Teachers[i].ShortName).ToList();
                var duplicatedUnitCount = timetableUnits.Select(u => u.StartAt).Distinct().Count();
                count += (timetableUnits.Count - duplicatedUnitCount);
            }
            return count;
        }

        private static int CheckH06(this TimetableIndividual src, TimetableParameters parameters)
        {
            var count = 0;
            for (var classIndex = 0; classIndex < src.Classes.Count; classIndex++)
            {
                var doublePeriodUnits = new List<TimetableUnitTCDTO>();
                for (var subjectIndex = 0; subjectIndex < parameters.DoublePeriodSubjects.Count; subjectIndex++)
                {
                    var result = false;
                    var timetableUnits = src.TimetableUnits
                        .Where(u => u.ClassName == src.Classes[classIndex].Name &&
                                    u.SubjectName == parameters.DoublePeriodSubjects[subjectIndex].Name)
                        .OrderBy(u => u.StartAt)
                        .ToList();
                    doublePeriodUnits.AddRange(timetableUnits);
                    for (var i = 0; i < timetableUnits.Count - 1; i++)
                        if (timetableUnits[i].StartAt == timetableUnits[i].StartAt - 1)
                        {
                            result = true;
                            break;
                        }
                    count += result ? 0 : 1;
                }
            }
            return count;
        }

        private static void CheckH07(this TimetableIndividual src) { }

        private static void CheckH08(this TimetableIndividual src) { }

        private static int CheckH09(this TimetableIndividual src, TimetableParameters parameters)
        {
            var count = 0;
            for (var classIndex = 0; classIndex < src.Classes.Count; classIndex++)
            {
                var doublePeriodUnits = new List<TimetableUnitTCDTO>();
                for (var subjectIndex = 0; subjectIndex < parameters.DoublePeriodSubjects.Count; subjectIndex++)
                    doublePeriodUnits.AddRange([.. src.TimetableUnits
                        .Where(u => u.ClassName == src.Classes[classIndex].Name &&
                                    u.SubjectName == parameters.DoublePeriodSubjects[subjectIndex].Name)
                        .OrderBy(u => u.StartAt)]);

                var singlePeriodUnits = src.TimetableUnits
                    .Where(u => u.ClassName == src.Classes[classIndex].Name && !doublePeriodUnits.Contains(u))
                    .OrderBy(u => u.StartAt)
                    .ToList();
                for (var i = 0; i < singlePeriodUnits.Count - 1; i++)
                    if (singlePeriodUnits[i].StartAt == singlePeriodUnits[i + 1].StartAt - 1 &&
                        singlePeriodUnits[i].SubjectName == singlePeriodUnits[i + 1].SubjectName)
                        count++;
            }
            return count;
        }

        private static void CheckH10(this TimetableIndividual src) { }

        private static int CheckH11(this TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var timetableUnits = src.TimetableUnits
                    .Where(u => u.TeacherName == src.Teachers[i].ShortName)
                    .OrderBy(u => u.StartAt)
                    .ToList();
                var count2 = 7;
                for (var j = 1; j < 61; j += 10)
                    if (timetableUnits.Any(u => u.StartAt >= j && u.StartAt <= j + 9))
                        count2--;
                if (count < 2)
                    count++;
            }
            return count;
        }

        #endregion

        #region LocalSeach

        private readonly struct Move(int step, string className, int startAt1, int startAt2)
        {
            public int Step { get; } = step;
            public string ClassName { get; } = className;
            public int StartAt1 { get; } = startAt1;
            public int StartAt2 { get; } = startAt2;
        }

        internal static void TabuSearch(this TimetableIndividual src)
        {
            var tabuList = new List<Move>();
            for (var i = 0; i < 100; i++)
            {
                var candidateMoves = new List<Move>();
                for (var j = 0; j < 100; j++)
                {
                    var randClassName = src.Classes.Shuffle().First().Name;
                    var randStartAts = src.TimetableUnits
                        .Where(u => u.ClassName == randClassName)
                        .Select(u => u.StartAt)
                        .Shuffle()
                        .Take(2)
                        .ToList();
                    candidateMoves.Add(new Move(i, randClassName, randStartAts[0], randStartAts[1]));
                }

            }
        }

        #endregion

        #region ExportData
        public static void ToCsv(this TimetableIndividual src)
        {
            var path = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\Timetable.csv";
            var file = new StreamWriter(path);
            var columnCount = src.TimetableFlag.GetLength(0);
            var rowCount = src.TimetableFlag.GetLength(1);
            file.Write("Tiết/Lớp,");
            for (var i = 0; i < src.Classes.Count; i++)
            {
                file.Write("{0}", src.Classes[i].Name);
                file.Write(",");
            }
            file.WriteLine();

            for (int row = 1; row < rowCount; row++)
            {
                file.Write("{0}", row);
                file.Write(",");
                for (int column = 0; column < columnCount; column++)
                {
                    var unit = src.TimetableUnits.FirstOrDefault(u => u.StartAt == row && u.ClassName == src.Classes[column].Name);
                    file.Write($"{unit?.SubjectName} - {unit?.TeacherName}");
                    file.Write(",");
                }
                file.WriteLine();
            }
            file.Close();
        }

        public static void ToCsv(this ETimetableFlag[,] timetableFlag, List<ClassTCDTO> classes)
        {
            var path = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\TimetableFlag.csv";
            var file = new StreamWriter(path);
            var columnCount = timetableFlag.GetLength(0);
            var rowCount = timetableFlag.GetLength(1);
            file.Write("Tiết/Lớp,");
            for (var i = 0; i < classes.Count; i++)
            {
                file.Write("{0}", classes[i].Name);
                file.Write(",");
            }
            file.WriteLine();

            for (int row = 1; row < rowCount; row++)
            {
                file.Write("{0}", row);
                file.Write(",");
                for (int column = 0; column < columnCount; column++)
                {
                    file.Write("{0}", timetableFlag[column, row]);
                    file.Write(",");
                }
                file.WriteLine();
            }
            file.Close();
        }
        #endregion
    }
}
