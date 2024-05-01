using AutoMapper;
using Entities.DAOs;
using Entities.DTOs;
using Entities.Exceptions;
using Entities.RequestFeatures;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;


namespace Services.Implementation
{
    internal sealed class TeacherService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ITeacherService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<(IEnumerable<TeacherDTO> teachers, MetaData metaData)> GetAllTeachersAsync(TeacherParameters teacherParameters, bool trackChanges)
        {

            var teacheresWithMetaData = await _repository.TeacherRepository.GetAllTeachersAsync(teacherParameters, trackChanges);

            var teacheresDTO = _mapper.Map<IEnumerable<TeacherDTO>>(teacheresWithMetaData);


            return (teachers: teacheresDTO, metaData: teacheresWithMetaData.MetaData);

        }

        public async Task<TeacherDTO?> GetTeacherAsync(Guid teacherId, bool trackChanges)
        {
            var teacher = await GetTeacherAndCheckIfItExists(teacherId, trackChanges);

            var teacherDTO = _mapper.Map<TeacherDTO>(teacher);

            return teacherDTO;
        }

        public async Task<IEnumerable<TeacherDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var teacherEntities = await _repository.TeacherRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != teacherEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var teacheresToReturn = _mapper.Map<IEnumerable<TeacherDTO>>(teacherEntities);
            return teacheresToReturn;
        }

        public async Task<TeacherDTO> CreateTeacherAsync(TeacherForCreationDTO teacher)
        {
            var teacherEntity = _mapper.Map<Teacher>(teacher);

            _repository.TeacherRepository.CreateTeacher(teacherEntity);
            await _repository.UnitOfWork.SaveAsync();

            var teacherToReturn = _mapper.Map<TeacherDTO>(teacherEntity);

            return teacherToReturn;
        }

        public async Task UpdateTeacherAsync(Guid teacherId, TeacherForUpdateDTO teacherForUpdate, bool trackChanges)
        {
            var teacher = await GetTeacherAndCheckIfItExists(teacherId, trackChanges);

            _mapper.Map(teacherForUpdate, teacher);

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteTeacherAsync(Guid teacherId, bool trackChanges)
        {
            var teacher = await GetTeacherAndCheckIfItExists(teacherId, trackChanges);
            _repository.TeacherRepository.DeleteTeacher(teacher);
            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task<(IEnumerable<TeacherDTO> teachers, string ids)> CreateTeacherCollectionAsync(
            IEnumerable<TeacherForCreationDTO> teacherCollection)
        {
            if (teacherCollection is null)
                throw new TeacherCollectionBadRequest();

            var teacherEntities = _mapper.Map<IEnumerable<Teacher>>(teacherCollection);

            foreach (var teacher in teacherEntities)
            {
                _repository.TeacherRepository.CreateTeacher(teacher);
            }

            await _repository.UnitOfWork.SaveAsync();

            var teacherCollectionToReturn = _mapper.Map<IEnumerable<TeacherDTO>>(teacherEntities);

            var ids = string.Join(",", teacherCollectionToReturn.Select(c => c.Id));

            return (teachers: teacherCollectionToReturn, ids: ids);
        }

        public async Task DeleteTeacherCollectionAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var teacherEntities = await _repository.TeacherRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != teacherEntities.Count())
                throw new CollectionByIdsBadRequestException();

            foreach (var teacher in teacherEntities)
            {
                _repository.TeacherRepository.DeleteTeacher(teacher);
            }

            await _repository.UnitOfWork.SaveAsync();
        }

        private async Task<Teacher> GetTeacherAndCheckIfItExists(Guid teacherId, bool trackChanges)
        {
            var teacher = await _repository.TeacherRepository.GetTeacherAsync(teacherId, trackChanges);

            if (teacher is null)
                throw new TeacherNotFoundException(teacherId);

            return teacher;
        }


    }
}
