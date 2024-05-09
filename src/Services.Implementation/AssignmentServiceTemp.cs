using Contexts;
using Entities.Common;
using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;

namespace Services.Implementation
{
    public class AssignmentServiceTemp(HsmsDbContext context) : IAssignmentServiceTemp
    {
        private readonly HsmsDbContext _context = context;

        public void Create()
        {
            var classes = _context.Classes
                .Include(c => c.SubjectClasses)
                .AsNoTracking()
                .ToList();
            foreach (var @class in classes)
            {
                foreach (var subjectClasses in @class.SubjectClasses)
                {
                    _context.Add(new Assignment
                    {
                        Id = Guid.NewGuid(),
                        PeriodCount = subjectClasses.PeriodCount,
                        SchoolShift = (ESchoolShift)@class.SchoolShift!,
                        Semester = 1,
                        StartYear = 2023,
                        EndYear = 2024,
                        ClassId = subjectClasses.ClassId,
                        SubjectId = subjectClasses.SubjectId,
                    });
                }
            }
            _context.SaveChanges();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            var assignments = _context.Assignments.ToList();
            var classes = _context.Classes
                .Include(c => c.SubjectClasses)
                .ThenInclude(sc => sc.Subject)
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToList();
            var teachers = _context.Teachers.AsNoTracking().ToList();
            var homeroomSubjectShortNames = new List<string>() { "SH", "TN&HN", "NGLL" };
            foreach (var @class in classes)
            {
                foreach (var subjectClass in @class.SubjectClasses)
                {
                    if (homeroomSubjectShortNames.Contains(subjectClass.Subject.ShortName))
                    {
                        var assignment = assignments
                            .First(a => a.SubjectId == subjectClass.SubjectId &&
                                        a.ClassId == @class.Id);
                        assignment.TeacherId = @class.HomeroomTeacherId;
                    }
                }
            }
        }
    }
}
