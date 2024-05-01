﻿using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface ISubjectRepository
    {
        Task<PagedList<Subject>> GetAllSubjectsAsync(SubjectParameters subjectParameters, bool trackChanges);

        Task<Subject?> GetSubjectAsync(Guid subjectId, bool trackChanges);

        void CreateSubject(Subject subject);

        Task<IEnumerable<Subject>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteSubject(Subject subject);
    }
}