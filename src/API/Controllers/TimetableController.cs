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
    public class TimetableController(ITimetableService timetableService, HsmsDbContext context) : ControllerBase
    {
        private readonly ITimetableService _timetableService = timetableService;
        private readonly HsmsDbContext _context = context;

        [HttpGet("test")]
        public IActionResult CreateTimetableTest()
        {
            var classNames = new List<string>()
            {
                "10B1", "10B3", "10B5", "10B7", "10B8",
                "11A2", "11A4", "11A6", "11A8", "11A9",
                "12C1", "12C3", "12C5", "12C6", "12C7"
            };
            var classes = _context.Classes
                .Where(c => classNames.Contains(c.Name))
                .AsNoTracking().ToList();
            var tParameters = new TimetableParameters
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
                .Where(a => tParameters.ClassIds.Contains(a.ClassId) &&
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
                    Priority = EPriority.Fixed,
                    StartAt = startAt,
                    AssignmentId = assignment.Id
                };
                tParameters.FixedTimetableUnits.Add(timetableUnit);
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
                    tParameters.FreeTimetableUnits.Add(new TimetableUnitTCDTO()
                    {
                        ClassName = @class.Name,
                        StartAt = startAt,
                    });
            }
            var tcParameters = new TimetableCreatorParameters();
            _timetableService.Create(tParameters, tcParameters);
            return Ok();
        }

        [HttpGet("demo")]
        public IActionResult CreateTimetableDemo()
        {
            var classes = _context.Classes.AsNoTracking().ToList();
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
            var tcParameters = new TimetableCreatorParameters();
            _timetableService.Create(parameters, tcParameters);
            return Ok();
        }

        [HttpGet("demo2")]
        public IActionResult GetClassAssignment()
        {
            var subjectClasses = _context.SubjectClasses.FromSqlRaw(
                "SELECT * FROM SubjectClasses "
                //+ "RIGHT JOIN Subjects ON SubjectClasses.SubjectId = Subjects.Id "
                //+ "RIGHT JOIN Classes ON SubjectClasses.ClassId = Classes.Id"
                )
                .ToList();
            return Ok();
        }
    }
}
