using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface ITeacherRepository
    {
        Task<PagedList<Teacher>> GetAllTeachersAsync(TeacherParameters teacherParameters, bool trackChanges);

        Task<Teacher?> GetTeacherAsync(Guid? teacherId, bool trackChanges);

        void CreateTeacher(Teacher teacher);

        Task<IEnumerable<Teacher>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteTeacher(Teacher teacher);
    }
}
