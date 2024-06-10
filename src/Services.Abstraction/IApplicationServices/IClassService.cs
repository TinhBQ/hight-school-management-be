using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface IClassService
    {
        Task<(IEnumerable<ClassDTO> classes, MetaData metaData)> GetAllClassesAsync(ClassParameters classParameters, bool trackChanges, bool isInclude);

        Task<(IEnumerable<ClassToHomeroomAssignmentDTO> classDTOForHomeroomAssignment, MetaData metaData)> GetAllHomeroomAssignment(ClassParameters classParameters, bool trackChanges, bool isInclude);

        Task<(IEnumerable<ClassYearDTO> classes, MetaData metaData)> GetYearsAsync(ClassParameters classParameters, bool trackChanges, bool isInclude);

        Task<ClassDTO?> GetClassAsync(Guid classId, bool trackChanges);

        Task<IEnumerable<ClassDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<ClassDTO> classes, string ids)> CreateClassCollectionAsync(IEnumerable<ClassForCreationDTO> classCollection);

        Task<ClassDTO> CreateClassAsync(ClassForCreationDTO klass);

        Task UpdateClassAsync(Guid classId, ClassForUpdateDTO classForUpdate, bool trackChanges);

        Task UpdateClassToHomeroomAssignmentAsync(Guid classId, ClassToHomeroomAssignmentForUpdateDTO classToHomeroomAssignmentUpdate, bool trackChanges);

        Task UpdateClassToHomeroomAssignmentCollectionAsync(IEnumerable<ClassToHomeroomAssignmentForUpdateCollectionDTO> classToHomeroomAssignmentUpdates, bool trackChanges);

        Task DeleteClassAsync(Guid classId, bool trackChanges);

        Task DeleteClassCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);
    }
}
