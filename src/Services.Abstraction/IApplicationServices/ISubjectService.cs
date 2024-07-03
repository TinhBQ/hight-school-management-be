using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ISubjectService
    {
        Task<(IEnumerable<SubjectDTO> subjects, MetaData metaData)> GetAllSubjectsAsync(SubjectParameters subjectParameters, bool trackChanges);

        Task<IEnumerable<SubjectDTO>> GetUnassignedSubjectsByClassId(Guid classId, bool trackChanges);

        Task<IEnumerable<SubjectDTO>> GetAssignedSubjectsByClassId(Guid classId, bool trackChanges);

        Task<IEnumerable<SubjectDTO>> GetUnassignedSubjectsByTeacherId(Guid teacherId, bool trackChanges);

        Task<IEnumerable<SubjectDTO>> GetAssignedSubjectsByTeacherId(Guid teacherId, bool trackChanges);

        Task<SubjectDTO?> GetSubjectAsync(Guid subjectId, bool trackChanges);

        Task<IEnumerable<SubjectDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<SubjectDTO> subjects, string ids)> CreateSubjectCollectionAsync(IEnumerable<SubjectForCreationDTO> subjectCollection);

        Task<SubjectDTO> CreateSubjectAsync(SubjectForCreationDTO klass);

        Task UpdateSubjectAsync(Guid subjectId, SubjectForUpdateDTO subjectForUpdate, bool trackChanges);

        Task DeleteSubjectAsync(Guid subjectId, bool trackChanges);

        Task DeleteSubjectCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
