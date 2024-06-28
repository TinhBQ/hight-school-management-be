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
    public class SubjectTeacherRepository(HsmsDbContext repositoryContext) : RepositoryBase<SubjectTeacher>(repositoryContext), ISubjectTeacherRepository
    {
        public async Task<IEnumerable<SubjectTeacher>> GetAllSubjectTeacher(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

        public async Task<PagedList<SubjectTeacher>> GetAllSubjectTeacherWithPagedList(SubjectTeacherParameters subjectTeacherParameters, bool trackChanges, bool isInclude)
        {
            if (subjectTeacherParameters.teacherId == null)
            {
                var subjectTachers = await FindByCondition(c => !c.IsDeleted, trackChanges)
               .Search(subjectTeacherParameters.SearchTerm ?? "")
               .Sort(subjectTeacherParameters.OrderBy ?? "")
               .Skip((subjectTeacherParameters.PageNumber - 1) * subjectTeacherParameters.PageSize)
               .Take(subjectTeacherParameters.PageSize)
               .JoinTable(isInclude)
               .ToListAsync();

                var count = await FindByCondition(c => !c.IsDeleted, trackChanges)
                    .Search(subjectTeacherParameters.SearchTerm ?? "")
                    .CountAsync();

                return new PagedList<SubjectTeacher>(subjectTachers, count, subjectTeacherParameters.PageNumber, subjectTeacherParameters.PageSize);
            } else
            {
                var subjectTachers = await FindByCondition(c => !c.IsDeleted && c.TeacherId.Equals(subjectTeacherParameters.teacherId), trackChanges)
                .Search(subjectTeacherParameters.SearchTerm ?? "")
                .Sort(subjectTeacherParameters.OrderBy ?? "")
                .Skip((subjectTeacherParameters.PageNumber - 1) * subjectTeacherParameters.PageSize)
                .Take(subjectTeacherParameters.PageSize)
                .JoinTable(isInclude)
                .ToListAsync();

                var count = await FindByCondition(c => !c.IsDeleted && c.TeacherId.Equals(subjectTeacherParameters.teacherId), trackChanges)
                    .Search(subjectTeacherParameters.SearchTerm ?? "")
                    .CountAsync();
                
                return new PagedList<SubjectTeacher>(subjectTachers, count, subjectTeacherParameters.PageNumber, subjectTeacherParameters.PageSize);
            }
        }

        public async Task<IEnumerable<SubjectTeacher>> GetAllSubjectTeacherByTeacherId(Guid teacherId, bool trackChanges, bool isInclude)
        {
            var subjectTachers = await FindByCondition(c => !c.IsDeleted && c.TeacherId.Equals(teacherId), trackChanges)
                .JoinTable(isInclude)
                .ToListAsync();

            return subjectTachers;
        }

        public async Task<SubjectTeacher?> GetSubjectTeacher(Guid? id, bool trackChanges)
        {
            return await FindByCondition(c => !c.IsDeleted && c.Id.Equals(id), trackChanges)
                            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<SubjectTeacher>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(c => !c.IsDeleted && ids.Contains(c.Id), trackChanges).ToListAsync();

        public void CreateSubjectTeacher(SubjectTeacher subjectTeacher) => Create(subjectTeacher);

        public void DeleteSubjectTeacher(SubjectTeacher subjectTeacher) => Delete(subjectTeacher);

       
    }
}
