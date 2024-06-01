using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;

namespace Repositories.Implementation
{
    public class AssignmentRepository(HsmsDbContext repositoryContext) : RepositoryBase<Assignment>(repositoryContext), IAssignmentRepository
    {
        public void CreateAssignment(Assignment assignment) => Create(assignment);

        public void DeleteAssignment(Assignment assignment) => Delete(assignment);

        public async Task<PagedList<Assignment>> GetAllAssignmentWithPagedList(AssignmentParameters assignmentParameters, bool trackChanges)
        {
            var assignments = await FindAll(trackChanges)
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .FilterAssignments(assignmentParameters.StartYear, assignmentParameters.EndYear)
                .Skip((assignmentParameters.PageNumber - 1) * assignmentParameters.PageSize)
                .Take(assignmentParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).CountAsync();
            return new PagedList<Assignment>(assignments, count, assignmentParameters.PageNumber, assignmentParameters.PageSize);
        }

        public Task<Assignment?> GetAssignmentAsync(Guid assignmentId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Assignment>> GetAssignments(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Assignment>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
