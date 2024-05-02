using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITeacherService
    {
        Task<(IEnumerable<TeacherDTO> teachers, MetaData metaData)> GetAllTeachersAsync(TeacherParameters teacherParameters, bool trackChanges);

        Task<TeacherDTO?> GetTeacherAsync(Guid teacherId, bool trackChanges);

        Task<IEnumerable<TeacherDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<TeacherDTO> teachers, string ids)> CreateTeacherCollectionAsync(IEnumerable<TeacherForCreationDTO> teacherCollection);

        Task<TeacherDTO> CreateTeacherAsync(TeacherForCreationDTO klass);

        Task UpdateTeacherAsync(Guid teacherId, TeacherForUpdateDTO teacherForUpdate, bool trackChanges);

        Task DeleteTeacherAsync(Guid teacherId, bool trackChanges);

        Task DeleteTeacherCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
