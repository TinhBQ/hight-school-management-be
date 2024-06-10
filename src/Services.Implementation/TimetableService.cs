﻿using Contexts;
using Entities.Common;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;
using Services.Implementation.Extensions;
using System.Diagnostics;

/* NOTE
 * dùng lại những hàm check cũ, rồi thêm phương thức lấy những tiết bị lỗi.
 */

namespace Services.Implementation
{
    internal static class TimetableCreator
    {
        private static readonly Random rand = new();

        private readonly int NUMBER_OF_GENERATIONS = int.MaxValue;
        private readonly int INITIAL_NUMBER_OF_INDIVIDUALS = 1000;
        private readonly float MUTATION_RATE = 0.1f;
        private readonly ESelectionMethod SELECTION_METHOD = ESelectionMethod.RankSelection;
        private readonly ECrossoverMethod CROSSOVER_METHOD = ECrossoverMethod.SinglePoint;
        private readonly EChromosomeType CHROMOSOME_TYPE = EChromosomeType.ClassChromosome;
        private readonly EMutationType MUTATION_TYPE = EMutationType.Default;

        public TimetableDTO Generate(TimetableParameters parameters)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            var (classes, teachers, subjects, assignments, timetableFlags) = GetData(parameters);

            // Tạo cá thể gốc
            /* Cá thể gốc là cá thể được tạo ra đầu tiên và là cá thể mang giá trị mặc định và bất biến*/
            var root = CreateRootIndividual(classes, teachers, assignments, timetableFlags, parameters);

            // Tạo quần thể ban đầu và tính toán độ thích nghi
            var timetablePopulation = new List<TimetableIndividual>();
            for (var i = 0; i < INITIAL_NUMBER_OF_INDIVIDUALS; i++)
            {
                var individual = Clone(root);
                RandomlyAssign(individual, parameters);
                CalculateAdaptability(individual, parameters);
                timetablePopulation.Add(individual);
                //individual.ToCsv();
                //individual.TimetableFlag.ToCsv(individual.Classes);
            }

            // Tạo vòng lặp cho quá trình tiến hóa của quần thể
            for (var step = 1; step <= NUMBER_OF_GENERATIONS; step++)
            {
                if (timetablePopulation.First().Adaptability < 1000)
                    break;

                // Lai tạo
                /* Tournament */
                var timetableChildren = new List<TimetableIndividual>();
                var tournamentList = new List<TimetableIndividual>();

                for (var i = 0; i < timetablePopulation.Count; i++)
                    tournamentList.Add(timetablePopulation.Shuffle().Take(10).OrderBy(i => i.Adaptability).First());

                for (var k = 0; k < tournamentList.Count - 1; k += 2)
                {
                    var parent1 = tournamentList[k];
                    var parent2 = tournamentList[k + 1];

                    var children = Crossover(root, [parent1, parent2], parameters);

                    timetableChildren.AddRange(children);
                }

                // Chọn lọc
                timetablePopulation.AddRange(timetableChildren);
                // TabuSearch(timetablePopulation[0], parameters);
                timetablePopulation = timetablePopulation.Where(u => u.Age < u.Longevity).OrderBy(i => i.Adaptability).Take(100).ToList();

                var best = timetablePopulation.First();
                best.ConstraintErrors = [.. best.ConstraintErrors.OrderBy(e => e.Code)];

                Console.SetCursorPosition(0, 0);
                Console.Clear();
                Console.WriteLine(
                    $"step {step}, " +
                    $"best score {best.Adaptability}\n" +
                    $"errors: ");
                foreach (var error in best.ConstraintErrors.Take(30))
                    Console.WriteLine("  " + error.Description);
                if (best.ConstraintErrors.Count > 30)
                    Console.WriteLine("...");
                Console.WriteLine("time: " + sw.Elapsed.ToString());
            }

            var timetableFirst = timetablePopulation.OrderBy(i => i.Adaptability).First();
            timetableFirst.ToCsv();
            timetableFirst.TimetableFlag.ToCsv(timetableFirst.Classes);

            var result = _mapper.Map<TimetableDTO>(timetableFirst);
            result.Id = Guid.NewGuid();
            result.StartYear = parameters.StartYear;
            result.EndYear = parameters.EndYear;
            result.Semester = parameters.Semester;
            result.Name = "Thời khóa biểu mới";

            var timetableDb = _mapper.Map<Timetable>(result);
            foreach (var unit in timetableDb.TimetableUnits)
                unit.TimetableId = timetableDb.Id;
            _context.Add(timetableDb);
            _context.SaveChanges();

            Console.WriteLine(sw.Elapsed.ToString());
            sw.Stop();

            return result;
        }

        public TimetableDTO Get(Guid id, TimetableParameters parameters)
        {
            var timetableDb = _context.Timetables
                .Include(t => t.TimetableUnits)
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == id)
                ?? throw new Exception("Không tìm thấy thời khóa biểu");

            var (classes, teachers, subjects, assignments, timetableFlags) = GetData(parameters);
            var root = Clone(CreateRootIndividual(classes, teachers, assignments, timetableFlags, parameters));
            root.TimetableUnits = _mapper.Map<TimetableDTO>(timetableDb).TimetableUnits;
            CalculateAdaptability(root, parameters);

            return _mapper.Map<TimetableDTO>(timetableDb);
        }
        public TimetableDTO Check(TimetableDTO timetable)
        {
            throw new NotImplementedException();
        }

        public void Update(TimetableDTO timetable)
        {
            var timetableDb = _context.Timetables
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == timetable.Id)
                ?? throw new Exception("Không tìm thấy thời khóa biểu");

            _mapper.Map(timetable, timetableDb);

            _context.Update(timetableDb);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            if (!_context.Timetables.Any(t => t.Id == id))
                throw new Exception("Không tìm thấy thời khóa biểu");
            _context.Timetables.Remove(_context.Timetables.First(t => t.Id == id));
        }

        #region Timetable Creator

        private (
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

            /* Tạo danh sách các tiết đôi */
            for (var i = 0; i < classes.Count; i++)
            {
                var classTimetableUnits = timetableUnits.Where(u => u.ClassName == classes[i].Name).ToList();
                for (var j = 0; j < parameters.DoublePeriodSubjects.Count; j++)
                {
                    var dPeriods = classTimetableUnits
                        .Where(u => parameters.DoublePeriodSubjects[j].ShortName == u.SubjectName).Take(2).ToList();
                    for (var k = 0; k < dPeriods.Count; k++)
                    {
                        dPeriods[k].Priority = EPriority.Double;
                    }
                }
            }

            timetableUnits = [.. timetableUnits.OrderBy(u => u.ClassName)];

            return new TimetableRootIndividual(timetableFlags, timetableUnits, classes, teachers);
        }

        internal static TimetableIndividual Clone(this TimetableIndividual src)
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
            return new TimetableIndividual(timetableFlag, timetableUnits, src.Classes, src.Teachers) { Age = 1, Longevity = rand.Next(1, 5) };
        }

        internal static void RandomlyAssign(this TimetableIndividual src, TimetableParameters parameters)
        {
            for (var i = 0; i < src.TimetableFlag.GetLength(0); i++)
            {
                var startAts = new List<int>();
                for (var j = 1; j < src.TimetableFlag.GetLength(1); j++)
                    if (src.TimetableFlag[i, j] == ETimetableFlag.Unfilled)
                        startAts.Add(j);
                var consecs = new List<(int, int)>();
                for (var index = 0; index < startAts.Count - 1; index++)
                    if (startAts[index + 1] - startAts[index] == 1)
                        consecs.Add((startAts[index], startAts[index + 1]));

                for (var j = 0; j < parameters.DoublePeriodSubjects.Count; j++)
                {
                    var periods = src.TimetableUnits
                        .Where(u => u.ClassName == src.Classes[i].Name &&
                                    u.SubjectName == parameters.DoublePeriodSubjects[j].ShortName &&
                                    u.Priority == EPriority.Double)
                        .Take(2)
                        .ToList();

                    var randConsecIndex = consecs.IndexOf(consecs.Shuffle().First());
                    periods[0].StartAt = consecs[randConsecIndex].Item1;
                    periods[1].StartAt = consecs[randConsecIndex].Item2;
                    src.TimetableFlag[i, periods[0].StartAt] = ETimetableFlag.Filled;
                    src.TimetableFlag[i, periods[1].StartAt] = ETimetableFlag.Filled;

                    if (randConsecIndex > 0 && randConsecIndex < consecs.Count - 1)
                        consecs.RemoveRange(randConsecIndex - 1, 3);
                    else if (randConsecIndex == consecs.Count - 1)
                        consecs.RemoveRange(randConsecIndex - 1, 2);
                    else
                        consecs.RemoveRange(randConsecIndex, 2);
                    startAts.Remove(periods[0].StartAt);
                    startAts.Remove(periods[1].StartAt);
                }

                var timetableUnits = src.TimetableUnits
                    .Where(u => u.ClassName == src.Classes[i].Name && u.StartAt == 0)
                    .Shuffle()
                    .ToList();
                startAts = startAts.Shuffle().ToList();
                if (startAts.Count != timetableUnits.Count) throw new Exception();
                for (var j = 0; j < timetableUnits.Count; j++)
                    timetableUnits[j].StartAt = startAts[j];
            }
        }

        #region Crossover Methods
        internal static List<TimetableIndividual> Crossover(
            this TimetableRootIndividual root,
            List<TimetableIndividual> parents,
            TimetableParameters tParameters,
            TimetableCreatorParameters tcParameters)
        {
            var children = new List<TimetableIndividual> { root.Clone(), root.Clone() };
            children[0] = root.Clone();
            children[1] = root.Clone();

            switch (tcParameters.CrossoverMethod)
            {
                case ECrossoverMethod.SinglePoint:
                    parents.SinglePointCrossover(children, EChromosomeType.ClassChromosome);
                    break;
                default:
                    throw new NotImplementedException();
            }


            // Đánh dấu lại timetableFlags
            children[0].RemarkTimetableFlag();
            children[1].RemarkTimetableFlag();

            // Đột biến
            children.Mutate(EChromosomeType.ClassChromosome, 0.5f);

            // Tính toán độ thích nghi
            children[0].CalculateAdaptability(tParameters);
            children[1].CalculateAdaptability(tParameters);

            return [children[0], children[1]];
        }

        private static List<TimetableIndividual> SinglePointCrossover(
            this List<TimetableIndividual> parents,
            List<TimetableIndividual> children,
            EChromosomeType chromosomeType)
        {
            var startIndex = 0;
            var endIndex = 0;
            switch (chromosomeType)
            {
                case EChromosomeType.ClassChromosome:
                    children[0].SortChromosome(EChromosomeType.ClassChromosome);
                    children[1].SortChromosome(EChromosomeType.ClassChromosome);
                    parents[0].SortChromosome(EChromosomeType.ClassChromosome);
                    parents[1].SortChromosome(EChromosomeType.ClassChromosome);
                    var className = parents[0].Classes[rand.Next(parents[0].Classes.Count)].Name;
                    startIndex = parents[0].TimetableUnits.IndexOf(parents[0].TimetableUnits.First(u => u.ClassName == className));
                    endIndex = startIndex + parents[0].Classes.First(c => c.Name == className).PeriodCount - 1;
                    break;
                case EChromosomeType.TeacherChromosome:
                    throw new NotImplementedException();
                // break;
                default:
                    throw new NotImplementedException();
            }
            // var randIndex = /*rand.Next(0, parents[0].TimetableUnits.Count)*/ parents[0].TimetableUnits.Count / 2;

            for (var i = 0; i < parents[0].TimetableUnits.Count; i++)
            {
                if (i < startIndex || i > endIndex)
                    for (var j = 0; j < children.Count; j++)
                    {
                        children[j].TimetableUnits[i].StartAt = parents[j].TimetableUnits[i].StartAt;
                        children[j].TimetableUnits[i].Priority = parents[j].TimetableUnits[i].Priority;
                    }
                else
                    for (var j = 0; j < children.Count; j++)
                    {
                        children[j].TimetableUnits[i].StartAt = parents[children.Count - 1 - j].TimetableUnits[i].StartAt;
                        children[j].TimetableUnits[i].Priority = parents[children.Count - 1 - j].TimetableUnits[i].Priority;
                    }
            }

            return children;
        }

        #endregion

        #region Mutation

        private static void Mutate(this List<TimetableIndividual> individuals, EChromosomeType type, float mutationRate)
        {
            for (var i = 0; i < individuals.Count; i++)
            {
                if (rand.Next(0, 100) > mutationRate * 100)
                    continue;

                var className = "";
                var teacherName = "";
                List<int> randNumList = null!;
                List<TimetableUnitTCDTO> timetableUnits = null!;

                switch (type)
                {
                    case EChromosomeType.ClassChromosome:
                        className = individuals[i].Classes[rand.Next(0, individuals[i].Classes.Count)].Name;
                        timetableUnits = individuals[i].TimetableUnits
                            .Where(u => u.ClassName == className && u.Priority != EPriority.Fixed).ToList();
                        randNumList = Enumerable.Range(0, timetableUnits.Count).Shuffle().ToList();
                        if (timetableUnits[randNumList[0]].Priority != EPriority.Double)
                            Swap(timetableUnits[randNumList[0]], timetableUnits[randNumList[1]]);
                        else
                        {
                            var doublePeriods = new List<TimetableUnitTCDTO>()
                            {
                                timetableUnits[randNumList[0]],
                                timetableUnits.First(
                                    u => u.SubjectName == timetableUnits[randNumList[0]].SubjectName &&
                                         u.Priority == timetableUnits[randNumList[0]].Priority)
                            };
                            randNumList.Remove(timetableUnits.IndexOf(doublePeriods[0]));
                            randNumList.Remove(timetableUnits.IndexOf(doublePeriods[1]));

                            doublePeriods = [.. doublePeriods.OrderBy(p => p.StartAt)];

                            var consecs = new List<(int, int)>();
                            randNumList.Sort();
                            for (var index = 0; index < randNumList.Count - 1; index++)
                                if (randNumList[index + 1] - randNumList[index] == 1)
                                    consecs.Add((randNumList[index], randNumList[index + 1]));

                            var randConsecIndex = consecs.IndexOf(consecs.Shuffle().First());

                            Swap(doublePeriods[0], timetableUnits[randConsecIndex]);
                            Swap(doublePeriods[1], timetableUnits[randConsecIndex + 1]);
                        }
                        //for (var j = 0; j < randNumList.Count - rand.Next(1, randNumList.Count - 1); j++)
                        //    Swap(timetableUnits[randNumList[j]], timetableUnits[randNumList[j + 1]]);
                        break;
                    case EChromosomeType.TeacherChromosome:
                        teacherName = individuals[i].Teachers[rand.Next(0, individuals[i].Teachers.Count)].ShortName;
                        timetableUnits = individuals[i].TimetableUnits
                            .Where(u => u.TeacherName == teacherName && u.Priority != EPriority.Fixed).ToList();
                        randNumList = Enumerable.Range(0, timetableUnits.Count).Shuffle().ToList();

                        for (var j = 0; j < randNumList.Count - rand.Next(1, randNumList.Count - 1); j++)
                            Swap(timetableUnits[randNumList[j]], timetableUnits[randNumList[j + 1]]);
                        break;
                }


            }
        }

        #endregion

        #region Enhance Solution

        private readonly struct Move(int step, string className, int startAt1, int startAt2)
        {
            public int Step { get; } = step;
            public string ClassName { get; } = className;
            public int StartAt1 { get; } = startAt1;
            public int StartAt2 { get; } = startAt2;
        }

        internal static void TabuSearch(this TimetableIndividual src, TimetableParameters parameters)
        {
            var tabu = new List<Move>();
            var candidates = new List<TimetableIndividual>();
            var classes = src.Classes.Shuffle().Take(rand.Next(1, src.Classes.Count / 2)).ToList();
            for (var i = 0; i < classes.Count; i++)
            {
                var timetableUnits = src.TimetableUnits.Where(u => u.ClassName == src.Classes[i].Name).Shuffle().ToList();
                for (var j = 0; j < timetableUnits.Count - 1; j += 2)
                {
                    var candidate = src.Clone();
                    var unit1 = candidate.TimetableUnits.First(u => u.Id == timetableUnits[j].Id);
                    var unit2 = candidate.TimetableUnits.First(u => u.Id == timetableUnits[j + 1].Id);
                    Swap(unit1, unit2);
                    candidate.CalculateAdaptability(parameters);
                    candidates.Add(candidate);
                }
            }
            candidates = candidates.Where(c => c.Adaptability < src.Adaptability).ToList();
            if (candidates.Count > 0)
                src = candidates.Shuffle().First();
        }

        #endregion

        #region TimetableFlag Remark

        private static void RemarkTimetableFlag(this TimetableIndividual src)
        {
            for (var i = 0; i < src.Classes.Count; i++)
                for (var j = 0; j < 61; j++)
                    if (src.TimetableFlag[i, j] != ETimetableFlag.None)
                        src.TimetableFlag[i, j] = ETimetableFlag.Unfilled;

            for (var i = 0; i < src.TimetableUnits.Count; i++)
            {
                var classIndex = src.Classes.IndexOf(src.Classes.First(c => c.Name == src.TimetableUnits[i].ClassName));
                if (src.TimetableUnits[i].Priority == EPriority.Fixed)
                    src.TimetableFlag[classIndex, src.TimetableUnits[i].StartAt] = ETimetableFlag.Fixed;
                else
                    src.TimetableFlag[classIndex, src.TimetableUnits[i].StartAt] = ETimetableFlag.Filled;
            }
        }

        #endregion

        #region Sort TimetableUnits by ChromosomeType

        private static void SortChromosome(this TimetableIndividual src, EChromosomeType type)
        {
            src.TimetableUnits = type switch
            {
                EChromosomeType.ClassChromosome => [.. src.TimetableUnits.OrderBy(u => u.ClassName)],
                EChromosomeType.TeacherChromosome => [.. src.TimetableUnits.OrderBy(u => u.TeacherName)],
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Fitness Function
        internal static void CalculateAdaptability(this TimetableIndividual src, TimetableParameters parameters)
        {
            src.TimetableUnits.ForEach(u => u.ConstraintErrors.Clear());
            src.ConstraintErrors.Clear();
            src.Adaptability =
                  CheckH03(src, parameters) * 1000
                + CheckH05(src) * 1000
                + CheckH06(src, parameters) * 10000
                + CheckH09(src) * 1000
                + CheckH11(src) * 10000;
            src.GetConstraintErrors();
        }

        private static int CheckH02(TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.TimetableUnits.Count; i++)
                if (src.TimetableUnits[i].StartAt <= 0)
                {
                    var errorMessage =
                        $"Lớp {src.TimetableUnits[i].ClassName}: " +
                        $"Môn {src.TimetableUnits[i].SubjectName} " +
                        $"chưa được phân công";
                    src.ConstraintErrors.Add(new()
                    {
                        Code = "H02",
                        ClassName = src.TimetableUnits[i].ClassName,
                        SubjectName = src.TimetableUnits[i].SubjectName,
                        Description = errorMessage
                    });
                    count++;
                }

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
                {
                    var errorMessage =
                        $"Lớp {fixedUnits[i].ClassName}: " +
                        $"Môn {fixedUnits[i].SubjectName} " +
                        $"xếp không đúng vị trí được định sẵn";
                    var error = new ConstraintError()
                    {
                        Code = "H03",
                        ClassName = fixedUnits[i].ClassName,
                        SubjectName = fixedUnits[i].SubjectName,
                        Description = errorMessage
                    };
                    fixedUnits[i].ConstraintErrors.Add(error);
                    count++;
                }
            return count;
        }

        private static void CheckH04(this TimetableIndividual src) { }

        private static int CheckH05(this TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var timetableUnits = src.TimetableUnits.Where(u => u.TeacherName == src.Teachers[i].ShortName).ToList();
                var errorMessage = "";

                for (var j = 0; j < timetableUnits.Count; j++)
                    if (timetableUnits.Count(u => u.StartAt == timetableUnits[j].StartAt) > 1)
                    {
                        var units = timetableUnits
                            .Where(u => u.StartAt == timetableUnits[j].StartAt)
                            .OrderBy(u => u.SubjectName)
                            .ToList();
                        var (day, period) = GetDayAndPeriod(timetableUnits[j].StartAt);
                        errorMessage =
                            $"Giáo viên {timetableUnits[j].TeacherName}: " +
                            $"dạy môn {string.Join(", ", units.Select(u => u.SubjectName))} " +
                            $"cùng lúc tại tiết {period} vào thứ {day}";
                        var error = new ConstraintError()
                        {
                            Code = "H05",
                            TeacherName = timetableUnits[j].TeacherName,
                            SubjectName = timetableUnits[j].SubjectName,
                            Description = errorMessage
                        };
                        timetableUnits[j].ConstraintErrors.Add(error);
                        count++;
                    }
            }
            return count;
        }

        private static int CheckH06(this TimetableIndividual src, TimetableParameters parameters)
        {
            var count = 0;
            for (var classIndex = 0; classIndex < src.Classes.Count; classIndex++)
            {
                var classTimetableUnits = src.TimetableUnits.Where(u => u.ClassName == src.Classes[classIndex].Name).ToList();
                for (var subjectIndex = 0; subjectIndex < parameters.DoublePeriodSubjects.Count; subjectIndex++)
                {
                    var doublePeriodUnits = classTimetableUnits
                        .Where(u => u.SubjectName == parameters.DoublePeriodSubjects[subjectIndex].ShortName &&
                                    u.Priority == EPriority.Double)
                        .OrderBy(u => u.StartAt)
                        .ToList();
                    if (doublePeriodUnits[0].StartAt != doublePeriodUnits[1].StartAt - 1)
                    {
                        var errorMessage =
                            $"Lớp {doublePeriodUnits[0].ClassName}: " +
                            $"Môn {doublePeriodUnits[0].SubjectName} " +
                            $"chưa được phân công tiết đôi";
                        var error = new ConstraintError()
                        {
                            Code = "H06",
                            ClassName = doublePeriodUnits[0].ClassName,
                            SubjectName = doublePeriodUnits[0].SubjectName,
                            Description = errorMessage
                        };
                        doublePeriodUnits[0].ConstraintErrors.Add(error);
                        doublePeriodUnits[1].ConstraintErrors.Add(error);
                        count += 2;
                    }
                }
            }
            return count;
        }

        private static void CheckH07(this TimetableIndividual src) { }

        private static void CheckH08(this TimetableIndividual src) { }

        private static int CheckH09(this TimetableIndividual src)
        {
            var count = 0;
            for (var classIndex = 0; classIndex < src.Classes.Count; classIndex++)
            {
                var classSingleTimetableUnits = src.TimetableUnits
                    .Where(u => u.ClassName == src.Classes[classIndex].Name && u.Priority != EPriority.Double)
                    .OrderBy(u => u.StartAt)
                    .ToList();
                for (var day = 2; day <= 7; day++)
                {
                    var singlePeriodUnits = classSingleTimetableUnits
                        .Where(u => u.StartAt >= (day - 2) * 10 + 1 &&
                                    u.StartAt <= (day - 2) * 10 + 5 &&
                                    u.Priority != EPriority.Double)
                        .ToList();
                    var doublePeriodUnits = classSingleTimetableUnits
                        .Where(u => u.StartAt >= (day - 2) * 10 + 1 &&
                                    u.StartAt <= (day - 2) * 10 + 5 &&
                                    u.Priority == EPriority.Double)
                        .ToList();
                    for (var i = 0; i < singlePeriodUnits.Count; i++)
                        if (singlePeriodUnits
                            .Count(u => u.SubjectName == singlePeriodUnits[i].SubjectName) > 1 ||
                            doublePeriodUnits.Select(s => s.SubjectName).Contains(singlePeriodUnits[i].SubjectName))
                        {
                            var errorMessage =
                            $"Lớp {singlePeriodUnits[i].ClassName}: " +
                            $"Môn {singlePeriodUnits[i].SubjectName} " +
                            $"chỉ được học một lần trong một buổi";
                            var error = new ConstraintError()
                            {
                                Code = "H09",
                                ClassName = singlePeriodUnits[i].ClassName,
                                SubjectName = singlePeriodUnits[i].SubjectName,
                                Description = errorMessage
                            };
                            singlePeriodUnits[i].ConstraintErrors.Add(error);
                            count += 1;
                        }
                }
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
                //var sessionCount = 0;
                //for (var j = 1; j < 61; j += 5)
                //    if (timetableUnits.Any(u => u.StartAt >= j && u.StartAt <= j + 4))
                //        sessionCount++;

                //if (12 - sessionCount < 2)
                //{
                //    var errorMessage =
                //        $"Giáo viên {src.Teachers[i].ShortName} " +
                //        $"phải có tối thiểu 2 buổi nghỉ trong tuần";
                //    var error = new ConstraintError()
                //    {
                //        Code = "H11",
                //        TeacherName = src.Teachers[i].ShortName,
                //        Description = errorMessage
                //    };
                //    src.ConstraintErrors.Add(error);
                //    count++;
                //}

                var dayCount = 7;
                for (var j = 1; j < 61; j += 10)
                    if (timetableUnits.Any(u => u.StartAt >= j && u.StartAt <= j + 9))
                        dayCount--;

                if (dayCount < 2)
                {
                    var errorMessage =
                        $"Giáo viên {src.Teachers[i].ShortName} " +
                        $"phải có tối thiểu 1 ngày nghỉ trong tuần";
                    var error = new ConstraintError()
                    {
                        Code = "H11",
                        TeacherName = src.Teachers[i].ShortName,
                        Description = errorMessage
                    };
                    src.ConstraintErrors.Add(error);
                    count++;
                }
            }
            return count;
        }

        #endregion

        #region ExportData
        public static void ToCsv(this TimetableIndividual src)
        {
            var path = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\Timetable.csv";
            var errorPath = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\TimetableError.txt";
            var file = new StreamWriter(path);
            var columnCount = src.TimetableFlag.GetLength(0);
            var rowCount = src.TimetableFlag.GetLength(1);
            file.Write("Tiết,");
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

            file = new StreamWriter(errorPath);
            for (var i = 0; i < src.Errors.Count; i++)
                file.WriteLine(src.Errors[i]);
            file.Close();
        }

        public static void ToCsv(this ETimetableFlag[,] timetableFlag, List<ClassTCDTO> classes)
        {
            var path = "D:\\Workspace\\dotnet-asp\\10-be\\TimetableFlag.csv";
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

        #region Enhance Solution

        /* Đừng có đổi 1 tiết 1 lần, đổi 1 mớ tiết 1 lần đi */
        private List<TimetableIndividual> TabuSearch(TimetableIndividual src, TimetableParameters parameters, string code)
        {
            var solutions = new List<TimetableIndividual>();
            switch (code)
            {
                case "H11":

                    break;
            }
            return solutions;
        }

        #endregion

        #region Utils

        private static void SortChromosome(TimetableIndividual src, EChromosomeType type)
        {
            src.TimetableUnits = type switch
            {
                EChromosomeType.ClassChromosome => [.. src.TimetableUnits.OrderBy(u => u.ClassName)],
                EChromosomeType.TeacherChromosome => [.. src.TimetableUnits.OrderBy(u => u.TeacherName)],
                _ => throw new NotImplementedException(),
            };
        }

        private static void RemarkTimetableFlag(TimetableIndividual src)
        {
            for (var i = 0; i < src.Classes.Count; i++)
                for (var j = 0; j < 61; j++)
                    if (src.TimetableFlag[i, j] != ETimetableFlag.None)
                        src.TimetableFlag[i, j] = ETimetableFlag.Unfilled;

            for (var i = 0; i < src.TimetableUnits.Count; i++)
            {
                var classIndex = src.Classes.IndexOf(src.Classes.First(c => c.Name == src.TimetableUnits[i].ClassName));
                if (src.TimetableUnits[i].Priority == EPriority.Fixed)
                    src.TimetableFlag[classIndex, src.TimetableUnits[i].StartAt] = ETimetableFlag.Fixed;
                else
                    src.TimetableFlag[classIndex, src.TimetableUnits[i].StartAt] = ETimetableFlag.Filled;
            }
        }

        private static (int day, int period) GetDayAndPeriod(int startAt)
        {
            var day = startAt / 10 + 1;
            var period = (startAt - 1) % 5 + 1;
            return (day, period);
        }
    }
}
