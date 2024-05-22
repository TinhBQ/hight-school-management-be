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
    internal sealed class SubjectService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ISubjectService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<(IEnumerable<SubjectDTO> subjects, MetaData metaData)> GetAllSubjectsAsync(SubjectParameters subjectParameters, bool trackChanges)
        {

            var subjectesWithMetaData = await _repository.SubjectRepository.GetAllSubjectWithPagedList(subjectParameters, trackChanges);

            var subjectesDTO = _mapper.Map<IEnumerable<SubjectDTO>>(subjectesWithMetaData);


            return (subjects: subjectesDTO, metaData: subjectesWithMetaData.metaData);

        }

        public async Task<IEnumerable<SubjectDTO>> GetUnassignedSubjectsByClassId(Guid classId, bool trackChanges)
        {
            await GetClassAndCheckIfItExists(classId, trackChanges);
            var d = await _repository.SubjectClassRepository.GetSubjectClassByClassId(classId, trackChanges);
            var s = await _repository.SubjectRepository.GetSubjects(trackChanges);
            var k = s.ToList().Where(s => !d.ToList().Select(x => x.SubjectId).Distinct().Contains(s.Id));
            var subjectesDTO = _mapper.Map<IEnumerable<SubjectDTO>>(k);

            return subjectesDTO;
        }

        public async Task<IEnumerable<SubjectDTO>> GetAssignedSubjectsByClassId(Guid classId, bool trackChanges)
        {
            await GetClassAndCheckIfItExists(classId, trackChanges);
            var subjectClasses = await _repository.SubjectClassRepository.GetSubjectClassByClassId(classId, trackChanges);
            var subjectDTOs = _mapper.Map<IEnumerable<SubjectDTO>>(subjectClasses.ToList().Select(x => x.Subject));
            return subjectDTOs;
        }

        public async Task<SubjectDTO?> GetSubjectAsync(Guid subjectId, bool trackChanges)
        {
            var subject = await GetSubjectAndCheckIfItExists(subjectId, trackChanges);

            var subjectDTO = _mapper.Map<SubjectDTO>(subject);

            return subjectDTO;
        }

        public async Task<IEnumerable<SubjectDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var subjectEntities = await _repository.SubjectRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != subjectEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var subjectesToReturn = _mapper.Map<IEnumerable<SubjectDTO>>(subjectEntities);
            return subjectesToReturn;
        }

        public async Task<SubjectDTO> CreateSubjectAsync(SubjectForCreationDTO subject)
        {
            var subjectEntity = _mapper.Map<Subject>(subject);

            _repository.SubjectRepository.CreateSubject(subjectEntity);
            await _repository.UnitOfWork.SaveAsync();

            var subjectToReturn = _mapper.Map<SubjectDTO>(subjectEntity);

            return subjectToReturn;
        }

        public async Task UpdateSubjectAsync(Guid subjectId, SubjectForUpdateDTO subjectForUpdate, bool trackChanges)
        {
            var subject = await GetSubjectAndCheckIfItExists(subjectId, trackChanges);

            _mapper.Map(subjectForUpdate, subject);

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteSubjectAsync(Guid subjectId, bool trackChanges)
        {
            var subject = await GetSubjectAndCheckIfItExists(subjectId, trackChanges);
            _repository.SubjectRepository.DeleteSubject(subject);
            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task<(IEnumerable<SubjectDTO> subjects, string ids)> CreateSubjectCollectionAsync(
            IEnumerable<SubjectForCreationDTO> subjectCollection)
        {
            if (subjectCollection is null)
                throw new SubjectCollectionBadRequest();

            var subjectEntities = _mapper.Map<IEnumerable<Subject>>(subjectCollection);

            foreach (var subject in subjectEntities)
            {
                _repository.SubjectRepository.CreateSubject(subject);
            }

            await _repository.UnitOfWork.SaveAsync();

            var subjectCollectionToReturn = _mapper.Map<IEnumerable<SubjectDTO>>(subjectEntities);

            var ids = string.Join(",", subjectCollectionToReturn.Select(c => c.Id));

            return (subjects: subjectCollectionToReturn, ids);
        }

        public async Task DeleteSubjectCollectionAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var subjectEntities = await _repository.SubjectRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != subjectEntities.Count())
                throw new CollectionByIdsBadRequestException();

            foreach (var subject in subjectEntities)
            {
                _repository.SubjectRepository.DeleteSubject(subject);
            }

            await _repository.UnitOfWork.SaveAsync();
        }

        private async Task<Subject> GetSubjectAndCheckIfItExists(Guid subjectId, bool trackChanges)
        {
            var subject = await _repository.SubjectRepository.GetSubjectAsync(subjectId, trackChanges);

            return subject is null ? throw new SubjectNotFoundException(subjectId) : subject;
        }

        private async Task<Class> GetClassAndCheckIfItExists(Guid classId, bool trackChanges)
        {
            var klass = await _repository.ClassRepository.GetClassAsync(classId, trackChanges);

            return klass is null ? throw new ClassNotFoundException(classId) : klass;
        }
    }
}
