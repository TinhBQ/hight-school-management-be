using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public class TeacherRepository(HsmsDbContext repositoryContext) : RepositoryBase<Teacher>(repositoryContext), ITeacherRepository
    {
        public async Task<PagedList<Teacher>> GetAllTeachersAsync(TeacherParameters teacherParameters, bool trackChanges)
        {
            var teacheres = await FindAll(trackChanges)
                .Search(teacherParameters.searchTerm ?? "")
                .Sort(teacherParameters.orderBy ?? "firstName")
                .Skip((teacherParameters.pageNumber - 1) * teacherParameters.pageSize)
                .Take(teacherParameters.pageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Teacher>(teacheres, count, teacherParameters.pageNumber, teacherParameters.pageSize);
        }

        public async Task<Teacher?> GetTeacherAsync(Guid TeacherId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(TeacherId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Teacher>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

        public void CreateTeacher(Teacher teacher) => Create(teacher);

        public void DeleteTeacher(Teacher teacher) => Delete(teacher);
    }
}
