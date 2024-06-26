using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface ISubjectTeacherRepository
    {
        Task<PagedList<SubjectTeacher>> GetAllSubjectTeacherWithPagedList(SubjectTeacherParameters subjectTeacherParameters, bool trackChanges, bool isInclude);

        Task<IEnumerable<SubjectTeacher>> GetAllSubjectTeacherByTeacherId(Guid teacherId, bool trackChanges, bool isInclude);

        Task<IEnumerable<SubjectTeacher>> GetAllSubjectTeacher(bool trackChanges);

        Task<SubjectTeacher?> GetSubjectTeacher(Guid? id, bool trackChanges);

        void CreateSubjectTeacher(SubjectTeacher subjectTeacher);

        Task<IEnumerable<SubjectTeacher>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteSubjectTeacher(SubjectTeacher subjectTeacher);
    }
}
