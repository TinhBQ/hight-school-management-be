using AutoMapper;
using Contexts;
using Entities.Common;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;

namespace API.Controllers
{
    [Route("api/timetables")]
    [ApiController]
    public class TimetableController(ITimetableService timetableService, IMapper mapper, HsmsDbContext context) : ControllerBase
    {
        private readonly ITimetableService _timetableService = timetableService;
        private readonly IMapper _mapper = mapper;
        private readonly HsmsDbContext _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Timetables.ToList());
        }

        [HttpGet("{id:guid}", Name = "TimetableById")]
        public IActionResult GetTimetable(Guid id)
        {
            var result = _timetableService.Get(id, CreateParameters());
            return Ok(result);
        }

        [HttpPost("test")]
        public IActionResult CreateTimetableTest()
        {
            var id = _timetableService.Generate(CreateParameters()).Id;

            return Ok(id);
        }

        [HttpPost]
        public IActionResult CreateTimetable(TimetableParameters parameters)
        {
            var timetable = _timetableService.Generate(parameters);
            return Ok(timetable);
        }

        [HttpPatch("checking")]
        public IActionResult CheckTimetable(TimetableIndividual timetable)
        {
            return Ok(_timetableService.Check(timetable));
        }

        [HttpPatch]
        public IActionResult UpdateTimetable(TimetableIndividual timetable)
        {
            _timetableService.Update(timetable);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] Guid id)
        {
            _timetableService.Delete(id);
            return Ok();
        }

        private TimetableParameters CreateParameters()
        {
            var classes = _context.Classes
                .AsNoTracking()
                .ToList();
            var dSubjects = new List<SubjectTCDTO>();
            _context.Subjects
                .Where(s => s.ShortName == "TOAN" || s.ShortName == "VAN" || s.ShortName == "NN")
                .AsNoTracking()
                .ToList()
                .ForEach(s => dSubjects.Add(new(s)));
            var parameters = new TimetableParameters
            {
                ClassIds = [.. classes.Select(c => c.Id)],
                DoublePeriodSubjects = dSubjects,
                MaxPeriodPerDay = 5,
                MinPeriodPerDay = 0,
                Semester = 1,
                StartYear = 2023,
                EndYear = 2024
            };
            var fixedSubjects = _context.Subjects
                .Where(s => s.ShortName == "SH" || s.ShortName == "TN&HN" || s.ShortName == "CC")
                .ToList();
            var fixedAssignments = _context.Assignments
                .Where(a => parameters.ClassIds.Contains(a.ClassId) &&
                            fixedSubjects.Select(s => s.Id).Contains(a.SubjectId))
                .Include(a => a.Subject)
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .AsNoTracking()
                .ToList();
            foreach (var assignment in fixedAssignments)
            {
                var startAt = assignment.SchoolShift == ESchoolShift.Morning ? 0 : 5;
                if (assignment.Subject.ShortName == "SH")
                    startAt += 55;
                else if (assignment.Subject.ShortName == "TN&HN")
                    startAt += 54;
                else if (assignment.Subject.ShortName == "CC")
                    startAt = assignment.SchoolShift == ESchoolShift.Morning ? 1 : 10;
                var timetableUnit = new TimetableUnitTCDTO()
                {
                    Id = Guid.NewGuid(),
                    Priority = EPriority.Fixed,
                    StartAt = startAt,
                    TeacherId = assignment.TeacherId == null ? Guid.Empty : (Guid)assignment.TeacherId,
                    TeacherName = assignment.Teacher.ShortName,
                    ClassId = assignment.ClassId,
                    ClassName = assignment.Class.Name,
                    SubjectId = assignment.Subject.Id,
                    SubjectName = assignment.Subject.ShortName,
                    AssignmentId = assignment.Id
                };
                parameters.FixedTimetableUnits.Add(timetableUnit);
            }
            foreach (var @class in classes)
            {
                var count = 30 - @class.PeriodCount;
                if (count > 5) throw new Exception();
                var a = @class.SchoolShift == ESchoolShift.Morning ? 0 : 5;
                var startAts = new List<int>() { 35 + a, 34 + a, 33 + a, 45 + a, 44 + a };
                for (int i = 0; i < 5 - count; i++)
                    startAts.RemoveAt(startAts.Count - 1);
                foreach (var startAt in startAts)
                    parameters.FreeTimetableUnits.Add(new TimetableUnitTCDTO()
                    {
                        Id = Guid.NewGuid(),
                        ClassId = @class.Id,
                        ClassName = @class.Name,
                        StartAt = startAt,
                        Priority = EPriority.High,
                    });
            }
            var subjectsWithPracticeRoom = _context.Subjects.AsNoTracking().First(s => s.ShortName == "TIN");
            parameters.SubjectsWithPracticeRoom.Add(subjectsWithPracticeRoom.Id, 2);
            // parameters.JsonOutput();

            return parameters;
        }
    }
}
