using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ISubjectTeacherService
    {
        Task<(IEnumerable<SubjectTeacherDTO> subjectTeacherDTO, MetaData metaData)> GetAllSubjectTeacher(SubjectTeacherParameters subjectTeacherParameters, bool trackChanges);

        Task<SubjectTeacherDTO?> GetSubjectTeacher(Guid id, bool trackChanges);

        Task<SubjectTeacherDTO> CreateSubjectTeacher(SubjectTeacherForCreationDTO subjectTeacher);

        Task<IEnumerable<SubjectTeacherDTO?>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<SubjectTeacherDTO> subjectTeacherDTO, string ids)> CreateSubjcetTeacherCollection(IEnumerable<SubjectTeacherForCreationDTO> subjectTeacherCollection);

        Task UpdateSubjectTeacher(Guid id, SubjectTeacherForUpdateDTO subjectTeacher, bool trackChanges);

        Task DeleteSubjectTeacher(Guid id, bool trackChanges);

        Task DeleteSubjectTeacherCollection(IEnumerable<Guid> ids, bool trackChanges);
    }
}
