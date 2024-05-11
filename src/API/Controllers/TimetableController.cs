using Contexts;
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

        [HttpGet("test")]
        public IActionResult CreateTimetableTest()
        {
            var classes = _context.Classes.AsNoTracking().ToList();
            var parameters = new TimetableParameters
            {
                ClassIds = [.. classes.Select(c => c.Id)],
                DoublePeriodSubjectIds = [.. _context.Subjects
                .Where(s => s.ShortName == "TOAN" || s.ShortName == "VAN" || s.ShortName == "NN")
                .Select(s => s.Id)],
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
                var startAt = assignment.SchoolShift == Entities.Common.ESchoolShift.Morning ? 0 : 5;
                if (assignment.Subject.ShortName == "SH")
                    startAt += 55;
                else if (assignment.Subject.ShortName == "TN&HN")
                    startAt += 54;
                else if (assignment.Subject.ShortName == "CC")
                    startAt = assignment.SchoolShift == Entities.Common.ESchoolShift.Morning ? 1 : 10;
                var timetableUnit = new TimetableUnitTCDTO()
                {
                    Priority = Entities.Common.EPriority.Fixed,
                    StartAt = startAt,
                    AssignmentId = assignment.Id
                };
                parameters.FixedTimetableUnits.Add(timetableUnit);
            }
            foreach (var @class in classes)
            {
                var count = 30 - @class.PeriodCount;
                if (count > 5) throw new Exception();
                var a = @class.SchoolShift == Entities.Common.ESchoolShift.Morning ? 0 : 5;
                var startAts = new List<int>() { 35 + a, 34 + a, 33 + a, 45 + a, 44 + a };
                for (int i = 0; i < 5 - count; i++)
                    startAts.RemoveAt(startAts.Count - 1);
                foreach (var startAt in startAts)
                    parameters.FreeTimetableUnits.Add(new TimetableUnitTCDTO()
                    {
                        ClassName = @class.Name,
                        Assignment = new AssignmentTCDTO(new ClassTCDTO(@class)),
                        StartAt = startAt,
                    });
            }
            _timetableService.Create(parameters);
            return Ok();
        }

        [HttpGet("demo")]
        public IActionResult CreateTimetableDemo()
        {
            var classes = _context.Classes.AsNoTracking().ToList();
            var parameters = new TimetableParameters
            {
                ClassIds = [.. classes.Select(c => c.Id)],
                DoublePeriodSubjectIds = [.. _context.Subjects
                .Where(s => s.ShortName == "TOAN" || s.ShortName == "VAN" || s.ShortName == "NN")
                .Select(s => s.Id)],
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
                var startAt = assignment.SchoolShift == Entities.Common.ESchoolShift.Morning ? 0 : 5;
                if (assignment.Subject.ShortName == "SH")
                    startAt += 55;
                else if (assignment.Subject.ShortName == "TN&HN")
                    startAt += 54;
                else if (assignment.Subject.ShortName == "CC")
                    startAt = assignment.SchoolShift == Entities.Common.ESchoolShift.Morning ? 1 : 10;
                var timetableUnit = new TimetableUnitTCDTO()
                {
                    Priority = Entities.Common.EPriority.Fixed,
                    StartAt = startAt,
                    AssignmentId = assignment.Id
                };
                parameters.FixedTimetableUnits.Add(timetableUnit);
            }
            foreach (var @class in classes)
            {
                var count = 30 - @class.PeriodCount;
                if (count > 5) throw new Exception();
                var a = @class.SchoolShift == Entities.Common.ESchoolShift.Morning ? 0 : 5;
                var startAts = new List<int>() { 35 + a, 34 + a, 33 + a, 45 + a, 44 + a };
                for (int i = 0; i < 5 - count; i++)
                    startAts.RemoveAt(startAts.Count - 1);
                foreach (var startAt in startAts)
                    parameters.FreeTimetableUnits.Add(new TimetableUnitTCDTO()
                    {
                        ClassName = @class.Name,
                        Assignment = new AssignmentTCDTO(new ClassTCDTO(@class)),
                        StartAt = startAt,
                    });
            }
            _timetableService.Create(parameters);
            return Ok();
        }
    }
}
