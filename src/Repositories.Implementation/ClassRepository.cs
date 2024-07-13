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
        public async Task<PagedList<Class>> GetAllClassWithPagedList(ClassParameters classParameters, bool trackChanges, bool isInclude)
        {
            var classes = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .FilterClasses(classParameters.StartYear, classParameters.EndYear)
                .FilterClassesWithGrade(classParameters.Grade)
                .FilterClassesWithIsAssignedHomeroom(classParameters.IsAssignedHomeroom)
                .Search(classParameters.SearchTerm ?? "")
                .Sort(classParameters.OrderBy ?? "name,homeroomTeacher.firstName")
                .Skip((classParameters.PageNumber - 1) * classParameters.PageSize)
                .Take(classParameters.PageSize)
                .JoinTable(isInclude)
                .ToListAsync();

            var count = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .FilterClasses(classParameters.StartYear, classParameters.EndYear)
                .FilterClassesWithGrade(classParameters.Grade)
                .FilterClassesWithIsAssignedHomeroom(classParameters.IsAssignedHomeroom)
                .Search(classParameters.SearchTerm ?? "")
                .CountAsync();

            return new PagedList<Class>(classes, count, classParameters.PageNumber, classParameters.PageSize);
        }

        public async Task<IEnumerable<Class>> GetClasses(bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted, trackChanges).ToListAsync();
        }

        public async Task<Class?> GetClassAsync(Guid? classId, bool trackChanges, bool isInclude) =>
            await FindByCondition(c => !c.IsDeleted && c.Id.Equals(classId), trackChanges)
            .JoinTable(isInclude)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Class>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && ids.Contains(c.Id), trackChanges)
            .ToListAsync();

        public void CreateClass(Class klass) => Create(klass);

        public void DeleteClass(Class klass) => Delete(klass);
    }
}
