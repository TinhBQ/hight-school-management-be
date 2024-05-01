using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public class ClassRepository(HsmsDbContext repositoryContext) : RepositoryBase<Class>(repositoryContext), IClassRepository
    {
        public async Task<PagedList<Class>> GetAllClassesAsync(ClassParameters classParameters, bool trackChanges)
        {
            var classes = await FindAll(trackChanges)
                .Search(classParameters.searchTerm ?? "")
                .Sort(classParameters.orderBy ?? "name")
                .Skip((classParameters.pageNumber - 1) * classParameters.pageSize)
                .Take(classParameters.pageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Class>(classes, count, classParameters.pageNumber, classParameters.pageSize);
        }

        public async Task<Class?> GetClassAsync(Guid classId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(classId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Class>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

        public void CreateClass(Class klass) => Create(klass);

        public void DeleteClass(Class klass) => Delete(klass);
    }
}
