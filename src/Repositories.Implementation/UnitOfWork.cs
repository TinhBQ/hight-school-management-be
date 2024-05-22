using Contexts;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    internal sealed class UnitOfWork(HsmsDbContext hsmsDbContext) : IUnitOfWork
    {

        private readonly HsmsDbContext _hsmsDbContext = hsmsDbContext;

        public async Task SaveAsync() => await _hsmsDbContext.SaveChangesAsync();
    }
}
