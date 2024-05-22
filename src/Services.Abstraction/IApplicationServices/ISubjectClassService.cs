using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ISubjectClassService
    {
        Task<(IEnumerable<SubjectClassDTO> subjectClasses, MetaData metaData)> GetAllSubjectClassesAsync(SubjectClassParameters subjectClassParameters, bool trackChanges);
        Task<(IEnumerable<SubjectClassDTO> subjectClasses, MetaData metaData)> GetSubjectClasByClassId(SubjectsForClassParameters subjectsForClassParameters, bool trackChanges);

        Task<SubjectClassDTO?> GetSubjectClassAsync(Guid id, bool trackChanges);

        Task<SubjectClassDTO> CreateSubjectClassAsync(SubjectClassForCreationDTO subjectClass);

        Task<IEnumerable<SubjectClassDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<SubjectClassDTO> subjectClasses, string ids)> CreateSubjcetClassCollectionAsync(IEnumerable<SubjectClassForCreationDTO> subjectClassCollection);

        Task UpdateSubjectClassAsync(Guid subjectClassId, SubjectClassForUpdateDTO subjectClassForUpdate, bool trackChanges);

        Task DeleteSubjectClassAsync(Guid subjectClassId, bool trackChanges);

        Task DeleteSubjectClassCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
