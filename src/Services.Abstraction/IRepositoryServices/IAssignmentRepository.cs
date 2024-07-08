using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface IAssignmentRepository
    {
        Task<PagedList<Assignment>> GetAllAssignmentWithPagedList(AssignmentParameters assignmentParameters, bool trackChanges, bool isInclude);

        Task<IEnumerable<Assignment>> GetAllAssignment(AssignmentParameters assignmentParameters, bool trackChanges, bool isInclude);

        Task<IEnumerable<Assignment>> GetAllAssignmentBySubjectId(Guid subjectId, AssignmentParameters assignmentParameters, bool trackChanges);

        Task<Assignment?> GetAssignmentAsync(Guid? id, bool trackChanges);

        Task<Assignment?> GetAssignmentAsync(Guid? classId, Guid? subjectId, bool trackChanges);

        void CreateAssignment(Assignment assignment);

        void DeleteAssignment(Assignment assignment);

        Task<IEnumerable<Assignment>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
