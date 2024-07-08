using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface IAssignmentBQTService
    {
        Task<(IEnumerable<AssignmentDTO>, MetaData)> GetAssignmentsAsync(AssignmentParameters parameters, bool trackChanges);

        Task<IEnumerable<AssignmentDTO>> GetAllAssignmentsAsync(AssignmentParameters parameters, bool trackChanges);

        Task<IEnumerable<ClassDTO>> GetAssignmentWithClasses(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<TeacherDTO>> GetAssignmentWithTeahers(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<SubjectDTO>> GetAssignmentWithSubjects(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<AssignmentSubjectDTO>> GetAssignmentWithSubjectsNotSameTeacher(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<AssignmentDTO?> GetAssignmentAsync(Guid id, bool trackChanges);


        Task<AssignmentDTO> CreateAssignmentAsync(AssignmentForCreationDTO assignment);

        Task DeleteAssignmentAsync(Guid id, bool trackChanges);

        Task UpdateAssignmentAsync(Guid id, AssignmentForUpdateDTO assignment, bool trackChanges);
    }
}
