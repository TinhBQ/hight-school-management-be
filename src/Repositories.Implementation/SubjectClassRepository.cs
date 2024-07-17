using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public class SubjectClassRepository(HsmsDbContext repositoryContext) : RepositoryBase<SubjectClass>(repositoryContext), ISubjectClassRepository
    {
        public async Task<PagedList<SubjectClass>> GetAllSubjectClassWithPagedList(SubjectClassParameters subjectClassParameters, bool trackChanges)
        {
            var subjectClasses = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(subjectClassParameters.SearchTerm ?? "")
                .Sort(subjectClassParameters.OrderBy ?? "")
                .Skip((subjectClassParameters.PageNumber - 1) * subjectClassParameters.PageSize)
                .Take(subjectClassParameters.PageSize)
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .ToListAsync();

            var count = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(subjectClassParameters.SearchTerm ?? "")
                .CountAsync();

            return new PagedList<SubjectClass>(subjectClasses, count, subjectClassParameters.PageNumber, subjectClassParameters.PageSize);
        }

        public async Task<PagedList<SubjectClass>> GetSubjectClassByClassIdWithPagedList(SubjectsForClassParameters subjectsForClassParameters, bool trackChanges)
        {
            var subjectClasses = await FindByCondition(c => !c.IsDeleted && c.ClassId.Equals(subjectsForClassParameters.classId), trackChanges)
                .Search(subjectsForClassParameters.SearchTerm ?? "")
                .Sort(subjectsForClassParameters.OrderBy ?? "")
                .Skip((subjectsForClassParameters.PageNumber - 1) * subjectsForClassParameters.PageSize)
                .Take(subjectsForClassParameters.PageSize)
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .ToListAsync();

            var count = await FindByCondition(c => !c.IsDeleted && c.ClassId.Equals(subjectsForClassParameters.classId), trackChanges)
                .Search(subjectsForClassParameters.SearchTerm ?? "")
                .CountAsync();

            return new PagedList<SubjectClass>(subjectClasses, count, subjectsForClassParameters.PageNumber, subjectsForClassParameters.PageSize);
        }

        public async Task<IEnumerable<SubjectClass>> GetAllSubjectClass(bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted, trackChanges).ToListAsync();
        }

        public async Task<IEnumerable<SubjectClass>> GetSubjectClassByClassId(Guid classId, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted && c.ClassId.Equals(classId), trackChanges)
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .ToListAsync();
        }

        public async Task<SubjectClass?> GetSubjectClassAsync(Guid? id, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && c.Id.Equals(id), trackChanges)
            .Include(s => s.Class)
                .ThenInclude(c => c.Assignments)
            .Include(s => s.Subject)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<SubjectClass>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && ids.Contains(c.Id), trackChanges)
            .ToListAsync();

        public void CreateSubjectClass(SubjectClass subjectClass) => Create(subjectClass);

        public void DeleteSubjectClass(SubjectClass subjectClass) => Delete(subjectClass);
    }
}
