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
            var teacheres = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(teacherParameters.SearchTerm ?? "")
                .FilterTeachersWithIsAssignedHomeroom(teacherParameters.IsAssignedHomeroom, teacherParameters.StartYear, teacherParameters.EndYear)
                .Sort(teacherParameters.OrderBy ?? "firstName")
                .Skip((teacherParameters.PageNumber - 1) * teacherParameters.PageSize)
                .Take(teacherParameters.PageSize)
                .Include(c => c.SubjectTeachers.Where(st => !st.IsDeleted))
                .ThenInclude(c => c.Subject)
                .ToListAsync();

            var count = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(teacherParameters.SearchTerm ?? "")
                .FilterTeachersWithIsAssignedHomeroom(teacherParameters.IsAssignedHomeroom, teacherParameters.StartYear, teacherParameters.EndYear)
                .CountAsync();

            return new PagedList<Teacher>(teacheres, count, teacherParameters.PageNumber, teacherParameters.PageSize);
        }

        public async Task<Teacher?> GetTeacherAsync(Guid? TeacherId, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && c.Id.Equals(TeacherId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Teacher>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && ids.Contains(c.Id), trackChanges)
            .ToListAsync();

        public void CreateTeacher(Teacher teacher) => Create(teacher);

        public void DeleteTeacher(Teacher teacher) => Delete(teacher);
    }
}
