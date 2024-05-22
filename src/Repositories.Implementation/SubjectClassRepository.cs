using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;
using System.Linq;

namespace Persistence.Repositories
{
    public class SubjectClassRepository(HsmsDbContext repositoryContext) : RepositoryBase<SubjectClass>(repositoryContext), ISubjectClassRepository
    {
        public async Task<PagedList<SubjectClass>> GetAllSubjectClassWithPagedList(SubjectClassParameters subjectClassParameters, bool trackChanges)
        {
            var subjectClasses = await FindAll(trackChanges)
                .Search(subjectClassParameters.SearchTerm ?? "")
                .Sort(subjectClassParameters.OrderBy ?? "")
                .Skip((subjectClassParameters.PageNumber - 1) * subjectClassParameters.PageSize)
                .Take(subjectClassParameters.PageSize)
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<SubjectClass>(subjectClasses, count, subjectClassParameters.PageNumber, subjectClassParameters.PageSize);
        }

        public async Task<PagedList<SubjectClass>> GetSubjectClassByClassIdWithPagedList(SubjectsForClassParameters subjectsForClassParameters, bool trackChanges)
        {
            var subjectClasses = await FindByCondition(x => x.ClassId.Equals(subjectsForClassParameters.classId), trackChanges)
                .Search(subjectsForClassParameters.SearchTerm ?? "")
                .Sort(subjectsForClassParameters.OrderBy ?? "")
                .Skip((subjectsForClassParameters.PageNumber - 1) * subjectsForClassParameters.PageSize)
                .Take(subjectsForClassParameters.PageSize)
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .ToListAsync();

            var count = await FindByCondition(x => x.ClassId.Equals(subjectsForClassParameters.classId), trackChanges)
                .Include(s => s.Class).Include(s => s.Subject).CountAsync();

            return new PagedList<SubjectClass>(subjectClasses, count, subjectsForClassParameters.PageNumber, subjectsForClassParameters.PageSize);
        }

        public async Task<IEnumerable<SubjectClass>> GetAllSubjectClass(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

        public async Task<IEnumerable<SubjectClass>> GetSubjectClassByClassId(Guid classId, bool trackChanges)
        {
            return await FindByCondition(x => x.ClassId.Equals(classId), trackChanges)
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .ToListAsync();
        }

        public async Task<SubjectClass?> GetSubjectClassAsync(Guid id, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(id), trackChanges)
            .Include(s => s.Class)
            .Include(s => s.Subject)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<SubjectClass>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

        public void CreateSubjectClass(SubjectClass subjectClass) => Create(subjectClass);

        public void DeleteSubjectClass(SubjectClass subjectClass) => Delete(subjectClass);
    }
}
