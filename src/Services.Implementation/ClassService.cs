using AutoMapper;
using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.Exceptions;
using Entities.RequestFeatures;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    internal sealed class ClassService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IHelperService helperService) : IClassService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHelperService _helperService = helperService;

        public async Task<(IEnumerable<ClassDTO> classes, MetaData metaData)> GetAllClassesAsync(ClassParameters classParameters, bool trackChanges, bool isInclude)
        {

            var classesWithMetaData = await _repository.ClassRepository.GetAllClassWithPagedList(classParameters, trackChanges, isInclude);

            var classesDTO = _mapper.Map<IEnumerable<ClassDTO>>(classesWithMetaData);

            return (classes: classesDTO, classesWithMetaData.metaData);

        }

        public async Task<(IEnumerable<ClassToHomeroomAssignmentDTO> classDTOForHomeroomAssignment, MetaData metaData)> GetAllHomeroomAssignment(ClassParameters classParameters, bool trackChanges, bool isInclude)
        {
            var classesWithMetaData = await _repository.ClassRepository.GetAllClassWithPagedList(classParameters, trackChanges, isInclude);

            var classForHomeroomAssignmentDTO = _mapper.Map<IEnumerable<ClassToHomeroomAssignmentDTO>>(classesWithMetaData);

            return (classDTOForHomeroomAssignment: classForHomeroomAssignmentDTO, metaData: classesWithMetaData.metaData);
        }

        public async Task<(IEnumerable<ClassYearDTO> classes, MetaData metaData)> GetYearsAsync(ClassParameters classParameters, bool trackChanges, bool isInclude)
        {
            var classesWithMetaData = await _repository.ClassRepository.GetAllClassWithPagedList(classParameters, trackChanges, isInclude);

            var classesDTO = _mapper.Map<IEnumerable<ClassYearDTO>>(classesWithMetaData);

            return (classes: classesDTO.Distinct(), metaData: classesWithMetaData.metaData);
        }

        public async Task<ClassDTO?> GetClassAsync(Guid classId, bool trackChanges)
        {
            var klass = await _helperService.GetClassAndCheckIfItExists(classId, trackChanges);

            var companyDto = _mapper.Map<ClassDTO>(klass);

            return companyDto;
        }

        public async Task<IEnumerable<ClassDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var classEntities = await _repository.ClassRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != classEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var classesToReturn = _mapper.Map<IEnumerable<ClassDTO>>(classEntities);
            return classesToReturn;
        }

        public async Task<ClassDTO> CreateClassAsync(ClassForCreationDTO klass)
        {
            var classEntity = _mapper.Map<Class>(klass);

            _repository.ClassRepository.CreateClass(classEntity);

            await _repository.UnitOfWork.SaveAsync();

            var companyToReturn = _mapper.Map<ClassDTO>(classEntity);

            return companyToReturn;
        }

        public async Task UpdateClassAsync(Guid classId, ClassForUpdateDTO classForUpdate, bool trackChanges)
        {
            var klass = await _helperService.GetClassAndCheckIfItExists(classId, trackChanges, true);
            _mapper.Map(classForUpdate, klass);

            foreach (var assignment in klass.Assignments)
            {
                assignment.SchoolShift = klass.SchoolShift;
                assignment.StartYear = klass.StartYear;
                assignment.EndYear = klass.EndYear;
            }

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task UpdateClassToHomeroomAssignmentAsync(Guid classId, ClassToHomeroomAssignmentForUpdateDTO classToHomeroomAssignmentUpdate, bool trackChanges)
        {
            var klass = await _helperService.GetClassAndCheckIfItExists(classId, trackChanges);

            var teacher = await _helperService.GetTeacherAndCheckIfItExists(classToHomeroomAssignmentUpdate.HomeroomTeacherId, trackChanges);

            klass.HomeroomTeacherId = teacher.Id;

            /* teacher.ClassId = null;

             _mapper.Map(classToHomeroomAssignmentUpdate, klass);

             var newTecher = await _helperService.GetTeacherAndCheckIfItExists(klass.HomeroomTeacherId, trackChanges);

             newTecher.Classes.Add(klass);*/

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task UpdateClassToHomeroomAssignmentCollectionAsync(IEnumerable<ClassToHomeroomAssignmentForUpdateCollectionDTO> classToHomeroomAssignmentForUpdateCollection, bool trackChanges)
        {
            if (classToHomeroomAssignmentForUpdateCollection is null)
                throw new ClassCollectionBadRequest();

            foreach (var classToHomeroomAssignment in classToHomeroomAssignmentForUpdateCollection)
            {
                Console.WriteLine(classToHomeroomAssignment.Id);
                var klass = await _helperService.GetClassAndCheckIfItExists(classToHomeroomAssignment.Id, trackChanges);

                if (klass.HomeroomTeacherId != Guid.Empty && klass.HomeroomTeacherId != null)
                {
                    var teacher = await _helperService.GetTeacherAndCheckIfItExists(klass.HomeroomTeacherId, trackChanges);

                    teacher.ClassId = null;
                }

                _mapper.Map(classToHomeroomAssignment, klass);

                var newTecher = await _helperService.GetTeacherAndCheckIfItExists(klass.HomeroomTeacherId, trackChanges);

                newTecher.ClassId = klass.Id;
            }

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteClassAsync(Guid classId, bool trackChanges)
        {
            var klass = await _helperService.GetClassAndCheckIfItExists(classId, trackChanges);

            klass.IsDeleted = true;

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task<(IEnumerable<ClassDTO> classes, string ids)> CreateClassCollectionAsync(
            IEnumerable<ClassForCreationDTO> classCollection)
        {
            if (classCollection is null)
                throw new ClassCollectionBadRequest();

            var classEntities = _mapper.Map<IEnumerable<Class>>(classCollection);

            foreach (var klass in classEntities)
            {
                _repository.ClassRepository.CreateClass(klass);
            }

            await _repository.UnitOfWork.SaveAsync();

            var classCollectionToReturn = _mapper.Map<IEnumerable<ClassDTO>>(classEntities);

            var ids = string.Join(",", classCollectionToReturn.Select(c => c.Id));

            return (classes: classCollectionToReturn, ids);
        }

        public async Task DeleteClassCollectionAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var classEntities = await _repository.ClassRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != classEntities.Count())
                throw new CollectionByIdsBadRequestException();

            foreach (var klass in classEntities)
            {

                klass.IsDeleted = true;
            }

            await _repository.UnitOfWork.SaveAsync();
        }
    }
}
