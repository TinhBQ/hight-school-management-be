﻿using AutoMapper;
using Entities.DAOs;
using Entities.DTOs;
using Entities.Exceptions;
using Entities.RequestFeatures;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    internal sealed class ClassService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IClassService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<(IEnumerable<ClassDTO> classes, MetaData metaData)> GetAllClassesAsync(ClassParameters classParameters, bool trackChanges)
        {

            var classesWithMetaData = await _repository.ClassRepository.GetAllClassesAsync(classParameters, trackChanges);

            var classesDTO = _mapper.Map<IEnumerable<ClassDTO>>(classesWithMetaData);

            _logger.LogInfo("classesDTO: " + classesDTO);

            return (classes: classesDTO, metaData: classesWithMetaData.MetaData);

        }

        public async Task<ClassDTO?> GetClassAsync(Guid classId, bool trackChanges)
        {
            var klass = await GetClassAndCheckIfItExists(classId, trackChanges);

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
            var klass = await GetClassAndCheckIfItExists(classId, trackChanges);

            _mapper.Map(classForUpdate, klass);

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteClassAsync(Guid classId, bool trackChanges)
        {
            var klass = await GetClassAndCheckIfItExists(classId, trackChanges);
            _repository.ClassRepository.DeleteClass(klass);
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
                _repository.ClassRepository.DeleteClass(klass);
            }

            await _repository.UnitOfWork.SaveAsync();
        }

        private async Task<Class> GetClassAndCheckIfItExists(Guid classId, bool trackChanges)
        {
            var company = await _repository.ClassRepository.GetClassAsync(classId, trackChanges);

            return company is null ? throw new ClassNotFoundException(classId) : company;
        }


    }
}