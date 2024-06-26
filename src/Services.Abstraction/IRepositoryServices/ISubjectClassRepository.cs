using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface ISubjectClassRepository
    {
        Task<PagedList<SubjectClass>> GetAllSubjectClassWithPagedList(SubjectClassParameters subjectClassParameters, bool trackChanges);

        Task<PagedList<SubjectClass>> GetSubjectClassByClassIdWithPagedList(SubjectsForClassParameters subjectsForClassParameters, bool trackChanges);

        Task<IEnumerable<SubjectClass>> GetAllSubjectClass(bool trackChanges);

        Task<IEnumerable<SubjectClass>> GetSubjectClassByClassId(Guid classId, bool trackChanges);

        Task<SubjectClass?> GetSubjectClassAsync(Guid? id, bool trackChanges);
        void CreateSubjectClass(SubjectClass subjectClass);

        Task<IEnumerable<SubjectClass>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteSubjectClass(SubjectClass subjectClass);
    }
}
