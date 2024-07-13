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
    internal sealed class SubjectTeacherService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IHelperService helperService) : ISubjectTeacherService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHelperService _helperService = helperService;

        public async Task<(IEnumerable<SubjectTeacherDTO> subjectTeacherDTO, MetaData metaData)> GetAllSubjectTeacher(SubjectTeacherParameters subjectTeacherParameters, bool trackChanges)
        {
            var subjectTeacherWithMetaData = await _repository.SubjectTeacherRepository.GetAllSubjectTeacherWithPagedList(subjectTeacherParameters, trackChanges, true);

            var subjectTeacherDTO = _mapper.Map<IEnumerable<SubjectTeacherDTO>>(subjectTeacherWithMetaData);

            return (subjectTeacherDTO, subjectTeacherWithMetaData.metaData);
        }

        public async Task<SubjectTeacherDTO?> GetSubjectTeacher(Guid id, bool trackChanges)
        {
            var subjectTeacher = await _helperService.GetSubjectTeacherAndCheckIfItExists(id, trackChanges);

            var subjectTeacherDto = _mapper.Map<SubjectTeacherDTO>(subjectTeacher);

            return subjectTeacherDto;
        }

        public async Task<IEnumerable<SubjectTeacherDTO?>> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null) throw new IdParametersBadRequestException();

            var subjectTeacherEntities = await _repository.SubjectTeacherRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != subjectTeacherEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var subjectTeachersToReturn = _mapper.Map<IEnumerable<SubjectTeacherDTO>>(subjectTeacherEntities);
            return subjectTeachersToReturn;
        }

        public async Task<SubjectTeacherDTO> CreateSubjectTeacher(SubjectTeacherForCreationDTO subjectTeacher)
        {
            var subjectTeacherEntity = _mapper.Map<SubjectTeacher>(subjectTeacher);

            _repository.SubjectTeacherRepository.CreateSubjectTeacher(subjectTeacherEntity);

            await _repository.UnitOfWork.SaveAsync();

            var subjectTeacherToReturn = _mapper.Map<SubjectTeacherDTO>(subjectTeacherEntity);

            return subjectTeacherToReturn;
        }

        public async Task UpdateSubjectTeacher(Guid id, SubjectTeacherForUpdateDTO subjectTeacher, bool trackChanges)
        {
            var subjectTeacherEntity = await _helperService.GetSubjectTeacherAndCheckIfItExists(id, trackChanges);

            _mapper.Map(subjectTeacher, subjectTeacherEntity);

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteSubjectTeacher(Guid id, bool trackChanges)
        {
            var subjectTeacherEntity = await _helperService.GetSubjectTeacherAndCheckIfItExists(id, trackChanges);

            subjectTeacherEntity.IsDeleted = true;

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task<(IEnumerable<SubjectTeacherDTO> subjectTeacherDTO, string ids)> CreateSubjcetTeacherCollection(IEnumerable<SubjectTeacherForCreationDTO> subjectTeacherCollection)
        {
            if (subjectTeacherCollection is null)
                throw new ClassCollectionBadRequest();

            var subjectTeacherEntities = _mapper.Map<IEnumerable<SubjectTeacher>>(subjectTeacherCollection);

            foreach (var subjectTeacherEntity in subjectTeacherEntities)
            {
                _repository.SubjectTeacherRepository.CreateSubjectTeacher(subjectTeacherEntity);
            }

            await _repository.UnitOfWork.SaveAsync();

            var subjectTeachersToReturn = _mapper.Map<IEnumerable<SubjectTeacherDTO>>(subjectTeacherEntities);

            var ids = string.Join(",", subjectTeachersToReturn.Select(c => c.Id));

            return (subjectTeacherDTO: subjectTeachersToReturn, ids);
        }

        public async Task DeleteSubjectTeacherCollection(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var subjectTeacherEntities = await _repository.SubjectTeacherRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != subjectTeacherEntities.Count())
                throw new CollectionByIdsBadRequestException();

            foreach (var subjectTeacherEntity in subjectTeacherEntities)
            {
                subjectTeacherEntity.IsDeleted = true;
            }

            await _repository.UnitOfWork.SaveAsync();
        }
    }
}
