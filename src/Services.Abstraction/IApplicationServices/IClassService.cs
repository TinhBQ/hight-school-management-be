﻿using Entities.DTOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface IClassService
    {
        Task<(IEnumerable<ClassDTO> classes, MetaData metaData)> GetAllClassesAsync(ClassParameters classParameters, bool trackChanges);

        Task<ClassDTO?> GetClassAsync(Guid classId, bool trackChanges);

        Task<IEnumerable<ClassDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<ClassDTO> classes, string ids)> CreateClassCollectionAsync(IEnumerable<ClassForCreationDTO> classCollection);

        Task<ClassDTO> CreateClassAsync(ClassForCreationDTO klass);

        Task UpdateClassAsync(Guid classId, ClassForUpdateDTO classForUpdate, bool trackChanges);

        Task DeleteClassAsync(Guid classId, bool trackChanges);

        Task DeleteClassCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}