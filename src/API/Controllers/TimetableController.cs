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
    public class TimetableController(ITimetableService timetableService, IAssignmentServiceTemp assignmentService, HsmsDbContext context) : ControllerBase
    {
        private readonly ITimetableService _timetableService = timetableService;
        private readonly HsmsDbContext _context = context;
        private readonly IAssignmentServiceTemp _assignmentService = assignmentService;

        [HttpGet]
        public IActionResult CreateTimetable()
        {
            var parameters = new TimetableParameters
            {
                ClassIds = [.. _context.Classes.Select(c => c.Id)],
                DoublePeriodSubjectIds = [.. _context.Subjects
                .Where(s => s.ShortName == "TOAN" || s.ShortName == "VAN" || s.ShortName == "NN")
                .Select(s => s.Id)],
                MaxPeriodPerDay = 5,
                MinPeriodPerDay = 0,
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
                    startAt += 1;
                var timetableUnit = new TimetableUnitTCDTO()
                {
                    Priority = Entities.Common.EPriority.Fixed,
                    StartAt = assignment.SchoolShift == Entities.Common.ESchoolShift.Morning ? 1 : 5,
                    AssignmentId = assignment.Id
                };
                parameters.FixedTimetableUnits.Add(timetableUnit);
            }
            _timetableService.Create(parameters);
            return Ok();
        }

        [HttpGet("AssignmentsCreation")]
        public IActionResult CreateAssignment()
        {
            _assignmentService.Create();
            return Ok();
        }
    }
}
