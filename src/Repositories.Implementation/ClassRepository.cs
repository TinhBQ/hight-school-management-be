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
        public async Task<PagedList<Class>> GetAllClassWithPagedList(ClassParameters classParameters, bool trackChanges)
        {
            var classes = await FindAll(trackChanges)
                .FilterClasses(classParameters.StartYear, classParameters.EndYear)
                .Search(classParameters.SearchTerm ?? "")
                .Sort(classParameters.OrderBy ?? "name")
                .Skip((classParameters.PageNumber - 1) * classParameters.PageSize)
                .Take(classParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Class>(classes, count, classParameters.PageNumber, classParameters.PageSize);
        }

        public async Task<IEnumerable<Class>> GetClasses(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
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
