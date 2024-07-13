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
    internal sealed class SubjectService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IHelperService helperService) : ISubjectService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHelperService _helperService = helperService;

        public async Task<(IEnumerable<SubjectDTO> subjects, MetaData metaData)> GetAllSubjectsAsync(SubjectParameters subjectParameters, bool trackChanges)
        {

            var subjectesWithMetaData = await _repository.SubjectRepository.GetAllSubjectWithPagedList(subjectParameters, trackChanges);

            var subjectesDTO = _mapper.Map<IEnumerable<SubjectDTO>>(subjectesWithMetaData);


            return (subjects: subjectesDTO, metaData: subjectesWithMetaData.metaData);

        }

        public async Task<IEnumerable<SubjectDTO>> GetUnassignedSubjectsByClassId(Guid classId, bool trackChanges)
        {
            await _helperService.GetClassAndCheckIfItExists(classId, trackChanges, false);

            var subjectsByClassId = await _repository.SubjectClassRepository.GetSubjectClassByClassId(classId, trackChanges);

            var subjects = await _repository.SubjectRepository.GetSubjects(trackChanges);

            var result = subjects.ToList().Where(subject => !subjectsByClassId.ToList().Select(x => x.SubjectId).Distinct().Contains(subject.Id));

            var subjectesDTO = _mapper.Map<IEnumerable<SubjectDTO>>(result);

            return subjectesDTO;
        }

        public async Task<IEnumerable<SubjectDTO>> GetAssignedSubjectsByClassId(Guid classId, bool trackChanges)
        {
            await _helperService.GetClassAndCheckIfItExists(classId, trackChanges);

            var subjectClasses = await _repository.SubjectClassRepository.GetSubjectClassByClassId(classId, trackChanges);

            var subjectDTOs = _mapper.Map<IEnumerable<SubjectDTO>>(subjectClasses.ToList().Select(x => x.Subject));

            return subjectDTOs;
        }

        public async Task<IEnumerable<SubjectDTO>> GetUnassignedSubjectsByTeacherId(Guid teacherId, bool trackChanges)
        {
            await _helperService.GetTeacherAndCheckIfItExists(teacherId, trackChanges);

            var subjectsByTeacherId = await _repository.SubjectTeacherRepository.GetAllSubjectTeacherByTeacherId(teacherId, trackChanges, true);

            var subjects = await _repository.SubjectRepository.GetSubjects(trackChanges);

            var result = subjects.ToList().Where(subject => !subjectsByTeacherId.ToList().Select(x => x.SubjectId).Distinct().Contains(subject.Id));

            var subjectesDTOs = _mapper.Map<IEnumerable<SubjectDTO>>(result);

            return subjectesDTOs;
        }

        public async Task<IEnumerable<SubjectDTO>> GetAssignedSubjectsByTeacherId(Guid teacherId, bool trackChanges)
        {
            await _helperService.GetTeacherAndCheckIfItExists(teacherId, trackChanges);

            var subjectClasses = await _repository.SubjectTeacherRepository.GetAllSubjectTeacherByTeacherId(teacherId, trackChanges, true);

            var subjectDTOs = _mapper.Map<IEnumerable<SubjectDTO>>(subjectClasses.ToList().Select(x => x.Subject));

            return subjectDTOs;
        }

        public async Task<SubjectDTO?> GetSubjectAsync(Guid subjectId, bool trackChanges)
        {
            var subject = await _helperService.GetSubjectAndCheckIfItExists(subjectId, trackChanges);

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
            var subject = await _helperService.GetSubjectAndCheckIfItExists(subjectId, trackChanges);

            _mapper.Map(subjectForUpdate, subject);

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteSubjectAsync(Guid subjectId, bool trackChanges)
        {
            var subject = await _helperService.GetSubjectAndCheckIfItExists(subjectId, trackChanges);

            subject.IsDeleted = true;

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
                subject.IsDeleted = true;
            }

            await _repository.UnitOfWork.SaveAsync();
        }
    }
}
