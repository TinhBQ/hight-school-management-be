using AutoMapper;
using Contexts;
using Entities.Common;
using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.IApplicationServices;
using Services.Implementation.Extensions;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json;

/* NOTE
 * dùng lại những hàm check cũ, rồi thêm phương thức lấy những tiết bị lỗi.
 */

namespace Services.Implementation
{
    public class TimetableService(HsmsDbContext context, IMapper mapper) : ITimetableService
    {
        private readonly HsmsDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly Random _random = new();

        private readonly int NUMBER_OF_GENERATIONS = int.MaxValue;
        private readonly int INITIAL_NUMBER_OF_INDIVIDUALS = 1000;
        private readonly float MUTATION_RATE = 0.1f;
        private readonly ESelectionMethod SELECTION_METHOD = ESelectionMethod.RankSelection;
        private readonly ECrossoverMethod CROSSOVER_METHOD = ECrossoverMethod.SinglePoint;
        private readonly EChromosomeType CHROMOSOME_TYPE = EChromosomeType.ClassChromosome;
        private readonly EMutationType MUTATION_TYPE = EMutationType.Default;

        public TimetableIndividual Generate(TimetableParameters parameters)
        {
            ValidateTimetableParameters(parameters);
            Stopwatch sw = Stopwatch.StartNew();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            var (classes, teachers, subjects, assignments, timetableFlags) = GetData(parameters);


            // Tạo cá thể gốc
            /* Cá thể gốc là cá thể được tạo ra đầu tiên và là cá thể mang giá trị mặc định và bất biến*/
            var root = CreateRootIndividual(classes, teachers, assignments, timetableFlags, parameters);

            // Tạo quần thể ban đầu và tính toán độ thích nghi
            var timetablePopulation = CreateInitialPopulation(root, parameters);

            // Tạo vòng lặp cho quá trình tiến hóa của quần thể
            var timetableIdBacklog = timetablePopulation.First().Id;
            var backlogCount = 0;
            var backlogCountMax = 0;

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
                var errors = best.ConstraintErrors.Where(error => error.IsHardConstraint == true).ToList();
                foreach (var error in errors.Take(20))
                    Console.WriteLine("  " + error.Description);
                if (errors.Count > 20)
                    Console.WriteLine("  ...");
                Console.WriteLine("warning: ");
                var warnings = best.ConstraintErrors.Where(error => error.IsHardConstraint == false).ToList();
                foreach (var error in warnings.Take(20))
                    Console.WriteLine("  " + error.Description);
                if (warnings.Count > 20)
                    Console.WriteLine("  ...");

                if (timetableIdBacklog == best.Id)
                {
                    backlogCount++;
                    if (backlogCount > 500)
                    {
                        timetablePopulation = CreateInitialPopulation(root, parameters);
                        backlogCountMax = backlogCount;
                        backlogCount = 0;
                        timetableIdBacklog = Guid.Empty;
                    }
                }
                else
                {
                    timetableIdBacklog = best.Id;
                    backlogCountMax = backlogCountMax < backlogCount ? backlogCount : backlogCountMax;
                    backlogCount = 0;
                }
                Console.WriteLine($"backlog count:  {backlogCount}\t max: {backlogCountMax}");
                Console.WriteLine("time: " + sw.Elapsed.ToString());
            }

            var timetableFirst = timetablePopulation.OrderBy(i => i.Adaptability).First();

            var timetableDb = _mapper.Map<Timetable>(timetableFirst);
            timetableFirst.Id = timetableDb.Id = Guid.NewGuid();
            timetableFirst.StartYear = timetableDb.StartYear = parameters.StartYear;
            timetableFirst.EndYear = timetableDb.EndYear = parameters.EndYear;
            timetableFirst.Semester = timetableDb.Semester = parameters.Semester;
            timetableFirst.Name = timetableDb.Name = "Thời khóa biểu mới";
            foreach (var unit in timetableDb.TimetableUnits)
                unit.TimetableId = timetableDb.Id;
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            timetableDb.Parameters = JsonSerializer.Serialize(parameters, jso);
            _context.Add(timetableDb);
            _context.SaveChanges();

            sw.Stop();
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            Console.WriteLine(sw.Elapsed.ToString() + ", " + backlogCountMax);

            //timetableFirst.ToCsv();
            //timetableFirst.TimetableFlag.ToCsv(timetableFirst.Classes);

            return timetableFirst;
        }

        public async Task<TimetableIndividual> Get(Guid id)
        {
            var timetableDb = _context.Timetables
                .Include(t => t.TimetableUnits)
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == id)
                ?? throw new Exception("Không tìm thấy thời khóa biểu");
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            var parameters = JsonSerializer.Deserialize<TimetableParameters>(timetableDb.Parameters, jso)
                ?? throw new Exception("Không tìm thấy dữ liệu cài đặt của thời khóa biểu");
            var (classes, teachers, subjects, assignments, timetableFlags) = await GetDataAsync(parameters);
            var root = Clone(CreateRootIndividual(classes, teachers, assignments, timetableFlags, parameters));
            root.Id = timetableDb.Id;
            root.TimetableUnits = _mapper.Map<TimetableDTO>(timetableDb).TimetableUnits;
            foreach (var unit in root.TimetableUnits)
            {
                var assigment = assignments
                    .FirstOrDefault(a => a.Teacher.Id == unit.TeacherId &&
                                a.Class.Id == unit.ClassId &&
                                a.Subject.Id == unit.SubjectId);
                if (assigment == null)
                    unit.AssignmentId = Guid.NewGuid();
                //throw new Exception("Không tìm thấy phân công");
                else
                    unit.AssignmentId = assigment.Id;
            };
            FixTimetableAfterUpdate(root, parameters);
            RemarkTimetableFlag(root);
            CalculateAdaptability(root, parameters);
            root.GetConstraintErrors();
            return root;
        }

        public async Task<TimetableIndividual> Check(Guid id)
        {
            var timetableDb = _context.Timetables
                .Include(t => t.TimetableUnits)
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == id)
                ?? throw new Exception("Không tìm thấy thời khóa biểu");
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            var parameters = JsonSerializer.Deserialize<TimetableParameters>(timetableDb.Parameters, jso)
                ?? throw new Exception("Không tìm thấy dữ liệu cài đặt của thời khóa biểu");
            var (classes, teachers, subjects, assignments, timetableFlags) = await GetDataAsync(parameters);
            var root = Clone(CreateRootIndividual(classes, teachers, assignments, timetableFlags, parameters));
            root.Id = timetableDb.Id;
            root.TimetableUnits = _mapper.Map<TimetableDTO>(timetableDb).TimetableUnits;
            foreach (var unit in root.TimetableUnits)
            {
                var assigment = assignments
                    .First(a => a.Teacher.Id == unit.TeacherId &&
                                a.Class.Id == unit.ClassId &&
                                a.Subject.Id == unit.SubjectId);
                unit.AssignmentId = assigment.Id;
            };

            FixTimetableAfterUpdate(root, parameters);
            RemarkTimetableFlag(root);
            CalculateAdaptability(root, parameters);
            root.GetConstraintErrors();
            return root;
        }

        public async Task Update(TimetableIndividual timetable)
        {
            var timetableDb = _context.Timetables
                .Include(t => t.TimetableUnits)
                .FirstOrDefault(t => t.Id == timetable.Id)
                ?? throw new Exception("Không tìm thấy thời khóa biểu");

            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            var parameters = JsonSerializer.Deserialize<TimetableParameters>(timetableDb.Parameters, jso)
                ?? throw new Exception("Không tìm thấy dữ liệu cài đặt của thời khóa biểu");
            var (classes, teachers, subjects, assignments, timetableFlags) = GetData(parameters);

            FixTimetableAfterUpdate(timetable, parameters);

            foreach (var unit in timetableDb.TimetableUnits)
            {
                var unitReq = timetable.TimetableUnits.First(u => u.Id == unit.Id);
                unit.StartAt = unitReq.StartAt;
                unit.Priority = unitReq.Priority;
            }


            var _ = await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var result = await _context.Timetables.AnyAsync(t => t.Id == id);
            if (!result) throw new Exception("Không tìm thấy thời khóa biểu");
            _context.Timetables.Remove(_context.Timetables.First(t => t.Id == id));
            await _context.SaveChangesAsync();
        }

        #region Timetable Creator

        private async Task<(
            List<ClassTCDTO>,
            List<TeacherTCDTO>,
            List<SubjectTCDTO>,
            List<AssignmentTCDTO>,
            ETimetableFlag[,]
            )> GetDataAsync(TimetableParameters parameters)
        {
            // Khởi tạo các biến
            var classes = new List<ClassTCDTO>();
            var teachers = new List<TeacherTCDTO>();
            var subjects = new List<SubjectTCDTO>();
            var assignments = new List<AssignmentTCDTO>();
            var timetableUnits = new List<TimetableUnitTCDTO>();
            ETimetableFlag[,] timetableFlags = null!;

            var classesDb = await _context.Classes
                    .Where(c => parameters.ClassIds.Contains(c.Id) &&
                                c.StartYear == parameters.StartYear &&
                                c.EndYear == parameters.EndYear &&
                                c.IsDeleted == false)
                    .Include(c => c.SubjectClasses.Where(x => x.IsDeleted == false))
                        .ThenInclude(sc => sc.Subject)
                    .OrderBy(c => c.Name)
                    .AsNoTracking()
                    .ToListAsync()
                    ?? throw new Exception();
            for (var i = 0; i < classesDb.Count; i++)
                classes.Add(new ClassTCDTO(classesDb[i]));
            if (classes.Count != parameters.ClassIds.Count)
                throw new Exception();

            timetableFlags = new ETimetableFlag[classes.Count, 61];

            var subjectsDb = await _context.Subjects.Where(s => s.IsDeleted == false).AsNoTracking().ToListAsync();
            for (var i = 0; i < subjectsDb.Count; i++)
                subjects.Add(new SubjectTCDTO(subjectsDb[i]));

            var assignmentsDb = await _context.Assignments
                .Where(a => a.StartYear == parameters.StartYear &&
                            a.EndYear == parameters.EndYear &&
                            a.Semester == parameters.Semester &&
                            a.IsDeleted == false &&
                            classesDb.Select(c => c.Id).Contains(a.ClassId))
                .AsNoTracking()
                .ToListAsync()
                ?? throw new Exception();

            var teacherIds = assignmentsDb.Select(a => a.TeacherId).Distinct().ToList();
            var teachersDb = await _context.Teachers
                .Where(t => teacherIds.Contains(t.Id) && t.IsDeleted == false)
                .OrderBy(c => c.LastName)
                .AsNoTracking()
                .ToListAsync()
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

        private (
            List<ClassTCDTO>,
            List<TeacherTCDTO>,
            List<SubjectTCDTO>,
            List<AssignmentTCDTO>,
            ETimetableFlag[,]
            ) GetData(TimetableParameters parameters)
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
                                c.EndYear == parameters.EndYear &&
                                c.IsDeleted == false)
                    .Include(c => c.SubjectClasses.Where(@class => @class.IsDeleted == false))
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
                            a.IsDeleted == false &&
                            classesDb.Select(c => c.Id).Contains(a.ClassId))
                .AsNoTracking()
                .ToList()
                ?? throw new Exception();

            var teacherIds = assignmentsDb.Select(a => a.TeacherId).Distinct().ToList();
            var teachersDb = _context.Teachers
                .Where(t => teacherIds.Contains(t.Id) && t.IsDeleted == false)
                .OrderBy(c => c.LastName)
                .AsNoTracking()
                .ToList()
                ?? throw new Exception();
            for (var i = 0; i < teachersDb.Count; i++)
                teachers.Add(new TeacherTCDTO(teachersDb[i]));

            for (var i = 0; i < assignmentsDb.Count; i++)
            {
                var @class = classes.First(c => c.Id == assignmentsDb[i].ClassId);
                var subject = subjects.First(s => s.Id == assignmentsDb[i].SubjectId);
                var teacher = teachers.First(t => t.Id == assignmentsDb[i].TeacherId);
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

        private static TimetableRootIndividual CreateRootIndividual(
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

            timetableUnits.AddRange(parameters.FixedTimetableUnits);
            //for (var i = 0; i < parameters.FixedTimetableUnits.Count; i++)
            //{
            //    var newUnit = new TimetableUnitTCDTO(assignments.First(a => a.Id == parameters.FixedTimetableUnits[i].AssignmentId))
            //    {
            //        Id = parameters.FixedTimetableUnits[i].Id,
            //        StartAt = parameters.FixedTimetableUnits[i].StartAt,
            //        Priority = parameters.FixedTimetableUnits[i].Priority,
            //    };
            //    timetableUnits.Add(newUnit);
            //}

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

        private TimetableIndividual Clone(TimetableIndividual src)
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
            return new TimetableIndividual(timetableFlag, timetableUnits, src.Classes, src.Teachers) { Age = 1, Longevity = _random.Next(1, 5) };
        }

        private static void RandomlyAssign(TimetableIndividual src, TimetableParameters parameters)
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

        #endregion

        #region Fitness Function
        private static void CalculateAdaptability(TimetableIndividual src, TimetableParameters parameters, bool isMinimized = false)
        {
            var rate = isMinimized ? 0 : 1;
            src.TimetableUnits.ForEach(u => u.ConstraintErrors.Clear());
            src.ConstraintErrors.Clear();
            src.Adaptability =
            CheckH03(src, parameters) * 1000
            + CheckH04AndH08(src, parameters) * 2000
            + CheckH05(src) * 1000
            + CheckH06(src, parameters) * 10000
            + CheckH09(src) * 1000
            + CheckH10(src, parameters) * 1000
            + CheckH11(src) * 1000;
            if (!isMinimized)
            {
                src.Adaptability +=
            CheckS01(src)
            + CheckS02(src)
            + CheckS03(src)
            + CheckS04(src);
            }

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

        private static int CheckH03(TimetableIndividual src, TimetableParameters parameters)
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

        private static int CheckH04AndH08(TimetableIndividual src, TimetableParameters parameters)
        {
            var count = 0;
            for (var i = 0; i < parameters.SubjectsWithPracticeRoom.Count; i++)
            {
                var timetableUnits = src.TimetableUnits
                    .Where(u => u.SubjectId == parameters.SubjectsWithPracticeRoom[i].Id)
                    .ToList();
                var group = timetableUnits
                    .GroupBy(u => u.StartAt)
                    .ToList();
                for (var j = 0; j < group.Count; j++)
                {
                    var units = group[j].Select(g => g).ToList();
                    if (units.Count > parameters.SubjectsWithPracticeRoom[i].RoomCount)
                    {
                        var (day, period) = GetDayAndPeriod(group[j].Key);
                        units.ForEach(u => u.ConstraintErrors.Add(new ConstraintError()
                        {
                            Code = "H04&H08",
                            ClassName = u.ClassName,
                            SubjectName = u.SubjectName,
                            Description = $"Không đủ phòng thực hành cho môn {u.SubjectName} tại tiết {period} vào thứ {day}"
                        }));
                        count += units.Count;
                    }
                }
            }
            return count;
        }

        private static int CheckH05(TimetableIndividual src)
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

        private static int CheckH06(TimetableIndividual src, TimetableParameters parameters)
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

        private static int CheckH09(TimetableIndividual src)
        {
            var count = 0;
            for (var classIndex = 0; classIndex < src.Classes.Count; classIndex++)
            {
                var classSingleTimetableUnits = src.TimetableUnits
                    .Where(u => u.ClassName == src.Classes[classIndex].Name)
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

        private static int CheckH10(TimetableIndividual src, TimetableParameters parameters)
        {
            var count = 0;
            for (var i = 0; i < parameters.NoAssignTimetableUnits.Count; i++)
            {
                var param = parameters.NoAssignTimetableUnits[i];
                var unit = src.TimetableUnits
                    .First(u => u.TeacherName == param.TeacherName &&
                                u.ClassName == param.ClassName &&
                                u.SubjectName == param.SubjectName);
                if (unit.StartAt == param.StartAt)
                {
                    var (day, period) = GetDayAndPeriod(unit.StartAt);
                    var errorMessage =
                            $"Lớp {unit.ClassName}: " +
                            $"Môn {unit.SubjectName} " +
                            $"không được xếp tại tiết {period} vào thứ {day}";
                    var error = new ConstraintError()
                    {
                        Code = "H10",
                        ClassName = unit.ClassName,
                        SubjectName = unit.SubjectName,
                        Description = errorMessage
                    };
                    unit.ConstraintErrors.Add(error);
                    count++;
                }

            }
            return count;
        }

        private static int CheckH11(TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var timetableUnits = src.TimetableUnits
                    .Where(u => u.TeacherName == src.Teachers[i].ShortName)
                    .OrderBy(u => u.StartAt)
                    .ToList();
                var sessionCount = 0;
                for (var j = 1; j < 61; j += 5)
                    if (timetableUnits.Any(u => u.StartAt >= j && u.StartAt <= j + 4))
                        sessionCount++;

                if (12 - sessionCount < 2)
                {
                    var errorMessage =
                        $"Giáo viên {src.Teachers[i].ShortName} " +
                        $"phải có tối thiểu 2 buổi nghỉ trong tuần";
                    var error = new ConstraintError()
                    {
                        Code = "H11",
                        TeacherName = src.Teachers[i].ShortName,
                        Description = errorMessage
                    };
                    src.ConstraintErrors.Add(error);
                    count++;
                }

                //var dayCount = 7;
                //for (var j = 1; j < 61; j += 10)
                //    if (timetableUnits.Any(u => u.StartAt >= j && u.StartAt <= j + 9))
                //        dayCount--;

                //if (dayCount < 2)
                //{
                //    var errorMessage =
                //        $"Giáo viên {src.Teachers[i].ShortName} " +
                //        $"phải có tối thiểu 1 ngày nghỉ trong tuần";
                //    var error = new ConstraintError()
                //    {
                //        Code = "H11",
                //        TeacherName = src.Teachers[i].ShortName,
                //        Description = errorMessage
                //    };
                //    src.ConstraintErrors.Add(error);
                //    count++;
                //}
            }
            return count;
        }

        private static int CheckS01(TimetableIndividual src)
        {
            var count = 0;
            var doublePeriods = src.TimetableUnits
                .Where(u => u.Priority == EPriority.Double)
                .OrderBy(u => u.ClassName)
                .ThenBy(u => u.StartAt)
                .ToList();
            var invalidMorningStartAts = new List<int>() { 2, 3 };
            var invalidAfternoonStartAts = new List<int>() { 8, 9 };

            for (var i = 0; i < doublePeriods.Count - 1; i++)
            {
                var current = doublePeriods[i];
                var next = doublePeriods[i + 1];
                var invalidStartAts = src.Classes.First(c => c.Name == current.ClassName).SchoolShift == ESchoolShift.Morning
                    ? invalidMorningStartAts : invalidAfternoonStartAts;

                if (invalidStartAts.Contains(current.StartAt % 10) &&
                    invalidStartAts.Contains(next.StartAt % 10) &&
                    current.ClassName == next.ClassName &&
                    current.SubjectName == next.SubjectName)
                {
                    current.ConstraintErrors.Add(new()
                    {
                        Code = "S01",
                        IsHardConstraint = false,
                        TeacherName = current.TeacherName,
                        ClassName = current.ClassName,
                        SubjectName = current.SubjectName,
                        Description = $"Lớp {current.ClassName}: " +
                        $"Môn {current.SubjectName} " +
                        $"nên tránh xếp vào các tiết 2,3 buổi sáng và 3,4 buổi chiều",
                    });
                    next.ConstraintErrors.Add(new()
                    {
                        Code = "S01",
                        IsHardConstraint = false,
                        TeacherName = next.TeacherName,
                        ClassName = next.ClassName,
                        SubjectName = next.SubjectName,
                        Description = $"Lớp {next.ClassName}: " +
                        $"Môn {next.SubjectName} " +
                        $"nên tránh xếp vào các tiết 2,3 buổi sáng và 3,4 buổi chiều",
                    });
                    count += 2;
                }
            }

            return count;
        }

        private static int CheckS02(TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var teacherPeriods = src.TimetableUnits
                    .Where(u => u.TeacherName == src.Teachers[i].ShortName)
                    .ToList();
                for (var j = 1; j < 60; j += 10)
                {
                    if (teacherPeriods.Any(p => p.StartAt >= j && p.StartAt < j + 10))
                        count++;
                }
            }
            return count;
        }

        private static int CheckS03(TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var teacherPeriods = src.TimetableUnits
                    .Where(u => u.TeacherName == src.Teachers[i].ShortName)
                    .OrderBy(u => u.StartAt)
                    .ToList();
                for (var j = 1; j < 60; j += 5)
                {
                    var periods = teacherPeriods
                        .Where(p => p.StartAt < j + 5 && p.StartAt >= j)
                        .OrderBy(p => p.StartAt)
                        .ToList();
                    for (var k = 0; k < periods.Count - 1; k++)
                        if (periods[k].StartAt != periods[k + 1].StartAt - 1)
                            count++;
                }
            }
            return count;
        }

        private static int CheckS04(TimetableIndividual src)
        {
            var count = 0;
            for (var i = 0; i < src.Teachers.Count; i++)
            {
                var teacherPeriods = src.TimetableUnits
                    .Where(u => u.TeacherName == src.Teachers[i].ShortName)
                    .OrderBy(u => u.StartAt)
                    .ToList();
                for (var j = 1; j < 60; j += 10)
                {
                    if (teacherPeriods.Any(p => p.StartAt >= j && p.StartAt < j + 5) &&
                        teacherPeriods.Any(p => p.StartAt >= j + 5 && p.StartAt < j + 10))
                    {
                        var periods = teacherPeriods
                            .Where(p => p.StartAt < j + 10 && p.StartAt >= j)
                            .ToList();
                        if (periods.Any(p => p.StartAt % 10 == 5 && p.StartAt % 10 == 6))
                            count++;
                    }
                }
            }
            return count;
        }

        #endregion

        #region Crossover Methods
        public List<TimetableIndividual> Crossover(
            TimetableRootIndividual root,
            List<TimetableIndividual> parents,
            TimetableParameters parameters)
        {
            var children = new List<TimetableIndividual> { Clone(root), Clone(root) };
            children[0] = Clone(root);
            children[1] = Clone(root);

            switch (CROSSOVER_METHOD)
            {
                case ECrossoverMethod.SinglePoint:
                    SinglePointCrossover(parents, children, EChromosomeType.ClassChromosome);
                    break;
                default:
                    throw new NotImplementedException();
            }


            // Đánh dấu lại timetableFlags
            RemarkTimetableFlag(children[0]);
            RemarkTimetableFlag(children[1]);

            // Đột biến
            Mutate(children, CHROMOSOME_TYPE, MUTATION_RATE);

            // Tính toán độ thích nghi
            CalculateAdaptability(children[0], parameters, true);
            CalculateAdaptability(children[1], parameters, true);

            return [children[0], children[1]];
        }

        private List<TimetableIndividual> SinglePointCrossover(
            List<TimetableIndividual> parents,
            List<TimetableIndividual> children,
            EChromosomeType chromosomeType)
        {
            var startIndex = 0;
            var endIndex = 0;
            switch (chromosomeType)
            {
                case EChromosomeType.ClassChromosome:
                    SortChromosome(children[0], EChromosomeType.ClassChromosome);
                    SortChromosome(children[1], EChromosomeType.ClassChromosome);
                    SortChromosome(parents[0], EChromosomeType.ClassChromosome);
                    SortChromosome(parents[1], EChromosomeType.ClassChromosome);
                    var className = parents[0].Classes[_random.Next(parents[0].Classes.Count)].Name;
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

        private void Mutate(List<TimetableIndividual> individuals, EChromosomeType type, float mutationRate)
        {
            for (var i = 0; i < individuals.Count; i++)
            {
                if (_random.Next(0, 100) > mutationRate * 100)
                    continue;

                var className = "";
                var teacherName = "";
                List<int> randNumList = null!;
                List<TimetableUnitTCDTO> timetableUnits = null!;

                switch (type)
                {
                    case EChromosomeType.ClassChromosome:
                        className = individuals[i].Classes[_random.Next(0, individuals[i].Classes.Count)].Name;
                        timetableUnits = individuals[i].TimetableUnits
                            .Where(u => u.ClassName == className && u.Priority != EPriority.Fixed).ToList();
                        randNumList = Enumerable.Range(0, timetableUnits.Count).Shuffle().ToList();
                        if (timetableUnits[randNumList[0]].Priority != EPriority.Double)
                            Utils.Swap(timetableUnits[randNumList[0]], timetableUnits[randNumList[1]]);
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

                            Utils.Swap(doublePeriods[0], timetableUnits[randConsecIndex]);
                            Utils.Swap(doublePeriods[1], timetableUnits[randConsecIndex + 1]);
                        }
                        //for (var j = 0; j < randNumList.Count - rand.Next(1, randNumList.Count - 1); j++)
                        //    Swap(timetableUnits[randNumList[j]], timetableUnits[randNumList[j + 1]]);
                        break;
                    case EChromosomeType.TeacherChromosome:
                        teacherName = individuals[i].Teachers[_random.Next(0, individuals[i].Teachers.Count)].ShortName;
                        timetableUnits = individuals[i].TimetableUnits
                            .Where(u => u.TeacherName == teacherName && u.Priority != EPriority.Fixed).ToList();
                        randNumList = Enumerable.Range(0, timetableUnits.Count).Shuffle().ToList();

                        for (var j = 0; j < randNumList.Count - _random.Next(1, randNumList.Count - 1); j++)
                            Utils.Swap(timetableUnits[randNumList[j]], timetableUnits[randNumList[j + 1]]);
                        break;
                }


            }
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

        private static void FixTimetableAfterUpdate(TimetableIndividual src, TimetableParameters parameters)
        {
            // fix tiết đôi
            var doubleSubjects = src.TimetableUnits
                .Where(u => parameters.DoublePeriodSubjects.Select(s => s.Id).Contains(u.SubjectId)).ToList();

            for (var i = 0; i < src.Classes.Count; i++)
            {
                for (var j = 0; j < parameters.DoublePeriodSubjects.Count; j++)
                {
                    var periods = doubleSubjects
                        .Where(u => u.ClassId == src.Classes[i].Id && u.SubjectId == parameters.DoublePeriodSubjects[j].Id)
                        .OrderBy(u => u.StartAt)
                        .ToList();

                    for (var k = 0; k < periods.Count - 1; k++)
                    {
                        var current = periods[k];
                        var next = periods[k + 1];
                        if (current.StartAt == next.StartAt - 1)
                        {
                            current.Priority = EPriority.Double;
                            next.Priority = EPriority.Double;
                            for (var l = k - 1; l >= 0; l--)
                                periods[l].Priority = EPriority.None;
                            for (var l = k + 2; l < periods.Count; l++)
                                periods[l].Priority = EPriority.None;
                            break;
                        }
                    }
                }
            }
        }

        private void ValidateTimetableParameters(TimetableParameters parameters)
        {
            if (parameters.ClassIds.IsNullOrEmpty())
                throw new Exception("Danh sách lớp học không được để trống");

            var classesNotFound = new List<string>();
            var classesDb = _context.Classes.AsNoTracking().ToList();
            parameters.ClassIds.ForEach(id =>
            {
                if (!classesDb.Any(c => c.Id == id))
                    classesNotFound.Add(id.ToString());
            });
            if (!classesNotFound.IsNullOrEmpty())
                throw new Exception("Không tìm thấy lớp học: " + string.Join(", ", classesNotFound));

            var unitNotAssigned = new List<string>();
            var assignmentDb = _context.Assignments
                .Where(a => a.StartYear == parameters.StartYear &&
                            a.EndYear == parameters.EndYear &&
                            a.Semester == parameters.Semester &&
                            a.IsDeleted == false)
                .AsNoTracking()
                .ToList();
            parameters.FixedTimetableUnits.ForEach(u =>
            {
                if (u.AssignmentId == Guid.Empty || !assignmentDb.Any(a => a.Id == u.AssignmentId))
                    unitNotAssigned.Add($"{u.ClassName}: {u.SubjectName}");
            });
            if (!unitNotAssigned.IsNullOrEmpty())
                throw new Exception("Không tìm thấy các tiết xếp sẵn này trong danh sách phân công: " + string.Join(", ", unitNotAssigned));
        }

        private List<TimetableIndividual> CreateInitialPopulation(
            TimetableRootIndividual root,
            TimetableParameters parameters)
        {
            var timetablePopulation = new List<TimetableIndividual>();
            for (var i = 0; i < INITIAL_NUMBER_OF_INDIVIDUALS; i++)
            {
                var individual = Clone(root);
                RandomlyAssign(individual, parameters);
                CalculateAdaptability(individual, parameters, true);
                timetablePopulation.Add(individual);
            }
            return timetablePopulation;
        }

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
                for (var j = 1; j < 61; j++)
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

        #endregion
    }
}
