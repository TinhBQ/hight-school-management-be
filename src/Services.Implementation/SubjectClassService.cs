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
    internal sealed class SubjectClassService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IHelperService helperService) : ISubjectClassService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IHelperService _helperService = helperService;

        public async Task<(IEnumerable<SubjectClassDTO> subjectClasses, MetaData metaData)> GetAllSubjectClassesAsync(SubjectClassParameters subjectClassParameters, bool trackChanges)
        {

            var subjectClassWithMetaData = await _repository.SubjectClassRepository.GetAllSubjectClassWithPagedList(subjectClassParameters, trackChanges);

            var subjectClassDTO = _mapper.Map<IEnumerable<SubjectClassDTO>>(subjectClassWithMetaData);

            return (subjectClasses: subjectClassDTO, metaData: subjectClassWithMetaData.metaData);
        }

        public async Task<(IEnumerable<SubjectClassDTO> subjectClasses, MetaData metaData)> GetSubjectClasByClassId(SubjectsForClassParameters subjectsForClassParameters, bool trackChanges)
        {
            await _helperService.GetClassAndCheckIfItExists(subjectsForClassParameters.classId, trackChanges);
            var subjectClassWithMetaData = await _repository.SubjectClassRepository.GetSubjectClassByClassIdWithPagedList(subjectsForClassParameters, trackChanges);
            var subjectClassDTO = _mapper.Map<IEnumerable<SubjectClassDTO>>(subjectClassWithMetaData);
            return (subjectClasses: subjectClassDTO, metaData: subjectClassWithMetaData.metaData);
        }

        public async Task<IEnumerable<dynamic>> GetAllSubjectClassForAssignment(bool trackChanges)
        {
            var classes = await _repository.ClassRepository.GetClasses(trackChanges);
            var subjects = await _repository.SubjectRepository.GetSubjects(trackChanges);
            var subjectClasses = await _repository.SubjectClassRepository.GetAllSubjectClass(trackChanges);

            var query = from subject in subjects
                        join subjectClass in subjectClasses on subject.Id equals subjectClass.SubjectId into subjectGroup
                        from subjectClass in subjectGroup.DefaultIfEmpty()

                        select new
                        {
                            SubjectClass = subjectClass,
                            Subject = subject,
                        };



            var classLeftJoinSubjectClass = classes.GroupJoin(
                subjectClasses,
                klass => klass.Id,
                subjectClass => subjectClass.ClassId,
                (klass, subjectClass) => new
                {
                    Class = klass,
                    SubjectClass = subjectClass.First(),
                }
            ).Select(result =>
            {
                if (result.SubjectClass != null)
                    result.SubjectClass.Class = result.Class;
                return subjectClasses;
            }) as IEnumerable<dynamic>;

            var k = subjects.GroupJoin(
                classLeftJoinSubjectClass,
                subject => subject.Id,
                subjectClass => subjectClass.SubjectId,
                (subject, subjectClass) => new
                {
                    Subject = subject,

                    SubjectClass = subjectClass.First(),
                }
            ).Select(result =>
            {
                if (result.SubjectClass != null && result.Subject != null)
                    result.SubjectClass.Subject = result.Subject;
                return classLeftJoinSubjectClass;
            }) as IEnumerable<SubjectClass>;

            System.Console.WriteLine(query.Count());

            return query;
        }

        public async Task<SubjectClassDTO?> GetSubjectClassAsync(Guid id, bool trackChanges)
        {
            var subjectClass = await _helperService.GetSubjectClassAndCheckIfItExists(id, trackChanges);

            var subjectClassDto = _mapper.Map<SubjectClassDTO>(subjectClass);

            return subjectClassDto;
        }

        public async Task<SubjectClassDTO> CreateSubjectClassAsync(SubjectClassForCreationDTO subjectClass)
        {
            var klass = await _helperService.GetClassAndCheckIfItExists(subjectClass.ClassId, true);

            await _helperService.GetSubjectAndCheckIfItExists(subjectClass.SubjectId, false);

            if (klass.PeriodCount + subjectClass.PeriodCount > 30)
            {
                throw new Exception("PeriodCount không hợp lệ");
            }

            var subjectClassEntity = _mapper.Map<SubjectClass>(subjectClass);

            klass.PeriodCount = klass.PeriodCount + subjectClass.PeriodCount;

            var assignmentDto1 = new AssignmentForCreationDTO();
            assignmentDto1.PeriodCount = subjectClass.PeriodCount;
            assignmentDto1.SchoolShift = klass.SchoolShift;
            assignmentDto1.SubjectId = subjectClass.SubjectId;
            assignmentDto1.ClassId = subjectClass.ClassId;
            assignmentDto1.StartYear = klass.StartYear;
            assignmentDto1.EndYear = klass.EndYear;
            assignmentDto1.Semester = 1;

            var assignment1Entity = _mapper.Map<Assignment>(assignmentDto1);

            _repository.SubjectClassRepository.CreateSubjectClass(subjectClassEntity);
            _repository.AssignmentRepository.CreateAssignment(assignment1Entity);

            await _repository.UnitOfWork.SaveAsync();

            var subjectClassToReturn = _mapper.Map<SubjectClassDTO>(subjectClassEntity);

            return subjectClassToReturn;
        }

        public async Task<IEnumerable<SubjectClassDTO?>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var subjectClassEntities = await _repository.SubjectClassRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != subjectClassEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var subjectClassToReturn = _mapper.Map<IEnumerable<SubjectClassDTO>>(subjectClassEntities);
            return subjectClassToReturn;
        }

        public async Task<(IEnumerable<SubjectClassDTO> subjectClasses, string ids)> CreateSubjcetClassCollectionAsync(IEnumerable<SubjectClassForCreationDTO> subjectClassCollection)
        {
            if (subjectClassCollection is null)
                throw new ClassCollectionBadRequest();

            var subjectClassEntities = _mapper.Map<IEnumerable<SubjectClass>>(subjectClassCollection);

            foreach (var subjectClass in subjectClassEntities)
            {
                await _helperService.GetClassAndCheckIfItExists(subjectClass.ClassId, false);

                await _helperService.GetSubjectAndCheckIfItExists(subjectClass.SubjectId, false);

                _repository.SubjectClassRepository.CreateSubjectClass(subjectClass);
            }

            await _repository.UnitOfWork.SaveAsync();

            var subjectClassCollectionToReturn = _mapper.Map<IEnumerable<SubjectClassDTO>>(subjectClassEntities);

            var ids = string.Join(",", subjectClassCollectionToReturn.Select(c => c.Id));

            return (subjectClasses: subjectClassCollectionToReturn, ids);
        }

        public async Task UpdateSubjectClassAsync(Guid subjectClassId, SubjectClassForUpdateDTO subjectClassForUpdate, bool trackChanges)
        {
            var subjectClass = await _helperService.GetSubjectClassAndCheckIfItExists(subjectClassId, trackChanges);

            _mapper.Map(subjectClassForUpdate, subjectClass);

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteSubjectClassAsync(Guid subjectClassId, bool trackChanges)
        {
            var subjectClass = await _helperService.GetSubjectClassAndCheckIfItExists(subjectClassId, trackChanges);
            subjectClass.Class.PeriodCount -= subjectClass.PeriodCount;
            subjectClass.IsDeleted = true;
            subjectClass.Class.Assignments.Where(a => a.SubjectId == subjectClass.SubjectId).ToList().ForEach(a =>
            {
                a.IsDeleted = true;
            });

            await _repository.UnitOfWork.SaveAsync();
        }

        public async Task DeleteSubjectClassCollectionAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var subjectClassEntities = await _repository.SubjectClassRepository.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != subjectClassEntities.Count())
                throw new CollectionByIdsBadRequestException();

            foreach (var subjectClass in subjectClassEntities)
            {
                subjectClass.IsDeleted = true;
            }

            await _repository.UnitOfWork.SaveAsync();
        }
    }
}
