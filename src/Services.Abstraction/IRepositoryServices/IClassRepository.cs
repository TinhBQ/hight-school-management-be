﻿using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface IClassRepository
    {
        Task<PagedList<Class>> GetAllClassesAsync(ClassParameters classParameters, bool trackChanges);

        Task<Class?> GetClassAsync(Guid classId, bool trackChanges);

        void CreateClass(Class klass);

        Task<IEnumerable<Class>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteClass(Class klass);
    }
}