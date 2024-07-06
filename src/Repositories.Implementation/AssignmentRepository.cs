using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation
{
    public class AssignmentRepository(HsmsDbContext repositoryContext) : RepositoryBase<Assignment>(repositoryContext), IAssignmentRepository
    {
        public void CreateAssignment(Assignment assignment) => Create(assignment);

        public void DeleteAssignment(Assignment assignment) => Delete(assignment);

        public async Task<IEnumerable<Assignment>> GetAllAssignment(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Include(a => a.Teacher)
                .Include(a => a.Subject)
                .Include(a => a.Class)
                .ThenInclude(a => a.HomeroomTeacher)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterClass(assignmentParameters.ClassId)
                .FilterSemester(assignmentParameters.Semester)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentBySubjectId(Guid subjectId, AssignmentParameters assignmentParameters, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted && c.SubjectId == subjectId, trackChanges)
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .ThenInclude(a => a.HomeroomTeacher)
                .Include(a => a.Subject)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterSemester(assignmentParameters.Semester)
                .ToListAsync();
        }

        public async Task<PagedList<Assignment>> GetAllAssignmentWithPagedList(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await FindAll(trackChanges)
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterClass(assignmentParameters.ClassId)
                .FilterSemester(assignmentParameters.Semester)
                .Skip((assignmentParameters.PageNumber - 1) * assignmentParameters.PageSize)
                .Take(assignmentParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterClass(assignmentParameters.ClassId)
                .FilterSemester(assignmentParameters.Semester)
                .CountAsync();

            return new PagedList<Assignment>(assignments, count, assignmentParameters.PageNumber, assignmentParameters.PageSize);
        }

        public async Task<IEnumerable<Class>> GetAssignmentWithClasses(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Include(a => a.Class)
                .ThenInclude(a => a.HomeroomTeacher)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterSemester(assignmentParameters.Semester)
                .ToListAsync();

            var distinctClass = assignments
                .GroupBy(a => a.ClassId)
                .Select(g => g.First().Class)
                .ToList();

            return distinctClass;
        }

        public async Task<IEnumerable<Class>> GetAssignmentWithClassesBySubjectId(Guid subjectId, AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await FindByCondition(c => !c.IsDeleted && c.SubjectId == subjectId, trackChanges)
                .Include(a => a.Class)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterSemester(assignmentParameters.Semester)
                .ToListAsync();

            var distinctClass = assignments
                .GroupBy(a => a.ClassId)
                .Select(g => g.First().Class)
                .ToList();

            return distinctClass;
        }

        public async Task<IEnumerable<Subject>> GetAssignmentWithSubjects(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Include(a => a.Subject)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterSemester(assignmentParameters.Semester)
                .ToListAsync();

            var distinctSubject = assignments
                .GroupBy(a => a.SubjectId)
                .Select(g => g.First().Subject)
                .ToList();

            return distinctSubject;
        }

        public async Task<IEnumerable<Subject>> GetAssignmentWithSubjectsNotSameTeacher(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            List<Subject> result = new List<Subject>();
            var subjects = await GetAssignmentWithSubjects(assignmentParameters, trackChanges);
            var klasses = await GetAssignmentWithClasses(assignmentParameters, trackChanges);

            foreach (var item in subjects)
            {
                var distinctTeachers = await FindByCondition(c => c.IsDeleted && c.SubjectId == item.Id, trackChanges)
                    .Include(a => a.Teacher)
                    .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                    .FilterSemester(assignmentParameters.Semester)
                    .Select(a => a.Teacher)
                    .Distinct()
                    .ToListAsync();

                if (!distinctTeachers.Any())
                {
                    result.Add(item);
                }
            }

            return result;
        }


        public async Task<IEnumerable<Teacher>> GetAssignmentWithTeahers(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Include(a => a.Teacher)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterSemester(assignmentParameters.Semester)
                .ToListAsync();

            var distinctTeachers = assignments
                .GroupBy(a => a.TeacherId)
                .Select(g => g.First().Teacher)
                .ToList();

            return distinctTeachers;
        }
    }
}
