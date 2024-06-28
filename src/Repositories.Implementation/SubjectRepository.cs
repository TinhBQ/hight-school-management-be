using Contexts;
using Entities.DAOs;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(HsmsDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<Subject>> GetAllSubjectWithPagedList(SubjectParameters subjectParameters, bool trackChanges)
        {
            var subjectes = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(subjectParameters.SearchTerm ?? "")
                .Sort(subjectParameters.OrderBy ?? "name")
                .Skip((subjectParameters.PageNumber - 1) * subjectParameters.PageSize)
                .Take(subjectParameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(c => !c.IsDeleted, trackChanges)
                .Search(subjectParameters.SearchTerm ?? "")
                .CountAsync();

            return new PagedList<Subject>(subjectes, count, subjectParameters.PageNumber, subjectParameters.PageSize);
        }

        public async Task<IEnumerable<Subject>> GetSubjects(bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted, trackChanges).ToListAsync();
        }

        public async Task<Subject?> GetSubjectAsync(Guid? SubjectId, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && c.Id.Equals(SubjectId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Subject>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && ids.Contains(c.Id), trackChanges)
            .ToListAsync();

        public  void CreateSubject(Subject subject) => Create(subject);

        public void DeleteSubject(Subject subject) => Delete(subject);
    }
}
