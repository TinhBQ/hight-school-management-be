using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation
{
    public class AssignmentRepository(HsmsDbContext repositoryContext) : RepositoryBase<Assignment>(repositoryContext), IAssignmentRepository
    {
        public async Task<IEnumerable<Assignment>> GetAllAssignment(AssignmentParameters assignmentParameters, bool trackChanges, bool isInclude)
        {
            return await FindByCondition(c => !c.IsDeleted, trackChanges)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterClass(assignmentParameters.ClassId)
                .FilterTeacher(assignmentParameters.TeacherId)
                .FilterSubject(assignmentParameters.SubjectId)
                .FilterSemester(assignmentParameters.Semester)
                .JoinTable(isInclude)
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

        public async Task<PagedList<Assignment>> GetAllAssignmentWithPagedList(AssignmentParameters assignmentParameters, bool trackChanges, bool isInclude)
        {
            var assignments = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(assignmentParameters.SearchTerm ??= "")
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterClass(assignmentParameters.ClassId)
                .FilterTeacher(assignmentParameters.TeacherId)
                .FilterSubject(assignmentParameters.SubjectId)
                .FilterSemester(assignmentParameters.Semester)
                .FilterSemesterAssigned(assignmentParameters.IsNotAssigned)
                .JoinTable(isInclude)
                .Skip((assignmentParameters.PageNumber - 1) * assignmentParameters.PageSize)
                .Take(assignmentParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges)
                .FilterYears(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .FilterClass(assignmentParameters.ClassId)
                .FilterTeacher(assignmentParameters.TeacherId)
                .FilterSubject(assignmentParameters.SubjectId)
                .FilterSemester(assignmentParameters.Semester)
                .FilterSemesterAssigned(assignmentParameters.IsNotAssigned)
                .CountAsync();

            return new PagedList<Assignment>(assignments, count, assignmentParameters.PageNumber, assignmentParameters.PageSize);
        }

        public async Task<Assignment?> GetAssignmentAsync(Guid? id, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted && c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Assignment>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted && ids.Contains(c.Id), trackChanges).ToListAsync();
        }

        public void CreateAssignment(Assignment assignment) => Create(assignment);

        public void DeleteAssignment(Assignment assignment) => Delete(assignment);

        public async Task<Assignment?> GetAssignmentAsync(Guid? classId, Guid? subjectId, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted && c.ClassId.Equals(classId) && c.SubjectId.Equals(subjectId), trackChanges).SingleOrDefaultAsync();
        }
    }
}
