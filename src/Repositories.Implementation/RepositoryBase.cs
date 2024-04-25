using Contexts;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IRepositoryServices;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public abstract class RepositoryBase<T>(HsmsDbContext repositoryContext) : IRepositoryBase<T> where T : class
    {
        protected HsmsDbContext HsmsDbContext = repositoryContext;

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            HsmsDbContext.Set<T>().AsNoTracking() :
            HsmsDbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
            HsmsDbContext.Set<T>().Where(expression).AsNoTracking() :
            HsmsDbContext.Set<T>().Where(expression);

        public void Create(T entity) => HsmsDbContext.Set<T>().Add(entity);

        public void Update(T entity) => HsmsDbContext.Set<T>().Update(entity);

        public void Delete(T entity) => HsmsDbContext.Set<T>().Remove(entity);
    }
}
