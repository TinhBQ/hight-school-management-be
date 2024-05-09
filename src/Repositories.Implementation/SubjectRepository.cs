﻿using Contexts;
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

        public async Task<PagedList<Subject>> GetAllSubjectsAsync(SubjectParameters subjectParameters, bool trackChanges)
        {
            var subjectes = await FindAll(trackChanges)
                .Search(subjectParameters.searchTerm ?? "")
                .Sort(subjectParameters.OrderBy ?? "name")
                .Skip((subjectParameters.PageNumber - 1) * subjectParameters.PageSize)
                .Take(subjectParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Subject>(subjectes, count, subjectParameters.PageNumber, subjectParameters.PageSize);
        }

        public async Task<Subject?> GetSubjectAsync(Guid SubjectId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(SubjectId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Subject>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

        public void CreateSubject(Subject subject) => Create(subject);

        public void DeleteSubject(Subject subject) => Delete(subject);
    }
}
