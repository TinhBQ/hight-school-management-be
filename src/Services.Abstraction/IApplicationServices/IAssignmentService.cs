using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface IAssignmentService
    {
        Task<(IEnumerable<AssignmentDTO>, MetaData)> GetAssignmentsAsync(AssignmentParameters parameters);
        Task<IEnumerable<AssignmentDTO>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<AssignmentDTO>> CreateAssignmentsAsync(IEnumerable<AssignmentForUpdateDTO> assignments);
        Task<IEnumerable<AssignmentDTO>> UpdateAssignmentsAsync(IEnumerable<AssignmentForUpdateDTO> assignments);
        Task DeleteAssignmentsAsync(IEnumerable<Guid> ids);
    }
}
