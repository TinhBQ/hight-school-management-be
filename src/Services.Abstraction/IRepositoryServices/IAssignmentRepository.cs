using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface IAssignmentRepository
    {
        Task<PagedList<Assignment>> GetAllAssignmentWithPagedList(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Assignment>> GetAssignments(bool trackChanges);

        Task<Assignment?> GetAssignmentAsync(Guid assignmentId, bool trackChanges);

        void CreateAssignment(Assignment assignment);

        Task<IEnumerable<Assignment>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteAssignment(Assignment assignment);
    }
}
