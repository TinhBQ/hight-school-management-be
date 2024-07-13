using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface IClassRepository
    {
        Task<PagedList<Class>> GetAllClassWithPagedList(ClassParameters classParameters, bool trackChanges, bool isInclude);

        Task<IEnumerable<Class>> GetClasses(bool trackChanges);

        Task<Class?> GetClassAsync(Guid? classId, bool trackChanges, bool isInclude);

        void CreateClass(Class klass);

        Task<IEnumerable<Class>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteClass(Class klass);
    }
}
