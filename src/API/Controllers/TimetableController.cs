using Contexts;
using Entities.Common;
using Entities.DTOs.CRUD;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;

namespace API.Controllers
{
    [Route("api/timetables")]
    [ApiController]
    public class TimetableController(ITimetableService timetableService, HsmsDbContext context) : ControllerBase
    {
        private readonly ITimetableService _timetableService = timetableService;
        private readonly HsmsDbContext _context = context;

        [HttpGet]
        public IActionResult Get([FromQuery] Guid id)
        {
            return Ok(_timetableService.Get(id, CreateParameters()));
        }

        [HttpPost("test")]
        public IActionResult CreateTimetableTest()
        {
            var timetable = _timetableService.Generate(CreateParameters());
            var id = timetable.Id;

            return Ok(id);
        }

        [HttpPost]
        public IActionResult CreateTimetable(TimetableParameters parameters)
        {
            var timetable = _timetableService.Generate(parameters);
            return Ok(timetable);
        }

        [HttpPatch("checking")]
        public IActionResult CheckTimetable(TimetableDTO timetable)
        {
            return Ok(_timetableService.Check(timetable));
        }

        [HttpPatch]
        public IActionResult UpdateTimetable(TimetableDTO timetable)
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
                .AsNoTracking().ToList();
            var parameters = new TimetableParameters
            {
                ClassIds = [.. classes.Select(c => c.Id)],
                DoublePeriodSubjects = [.. _context.Subjects
                .Where(s => s.ShortName == "TOAN" || s.ShortName == "VAN" || s.ShortName == "NN")],
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
                        ClassName = @class.Name,
                        StartAt = startAt,
                    });
            }
            parameters.SubjectsWithPracticeRoom.Add((_context.Subjects.AsNoTracking().First(s => s.ShortName == "TIN"), 2));
            return parameters;
        }
    }
}
