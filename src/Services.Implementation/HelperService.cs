using AutoMapper;
using Entities.DAOs;
using Entities.Exceptions;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    public class HelperService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IHelperService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Assignment> GetAssignmentAndCheckIfItExists(Guid? id, bool trackChanges)
        {
            if (id == null)
            {
                throw new AssignmentNotFoundException(Guid.Empty);
            }
            var assignment = await _repository.AssignmentRepository.GetAssignmentAsync(id, trackChanges);

            return assignment is null ? throw new AssignmentNotFoundException(id ?? Guid.Empty) : assignment;
        }

        public async Task<Assignment?> GetAssignmentAndCheckIfItExists(Guid? classId, Guid? subjectId, bool trackChanges)
        {
            if (classId == null || subjectId == null)
            {
                throw new AssignmentNotFoundException(Guid.Empty);
            }
            var assignment = await _repository.AssignmentRepository.GetAssignmentAsync(classId, subjectId, trackChanges);

            return assignment is null ? throw new AssignmentNotFoundException(classId ?? Guid.Empty) : assignment;
        }

        public async Task<Class> GetClassAndCheckIfItExists(Guid? id, bool trackChanges, bool isInclude = false)
        {
            if (id == null)
            {
                throw new ClassNotFoundException(Guid.Empty);
            }
            var klass = await _repository.ClassRepository.GetClassAsync(id, trackChanges, isInclude);
            return klass is null ? throw new ClassNotFoundException(id ?? Guid.Empty) : klass;
        }

        public async Task<Subject> GetSubjectAndCheckIfItExists(Guid? id, bool trackChanges)
        {
            if (id == null)
            {
                throw new SubjectNotFoundException(Guid.Empty);
            }
            var subject = await _repository.SubjectRepository.GetSubjectAsync(id, trackChanges);
            return subject is null ? throw new SubjectNotFoundException(id ?? Guid.Empty) : subject;
        }

        public async Task<SubjectClass> GetSubjectClassAndCheckIfItExists(Guid? id, bool trackChanges)
        {
            if (id == null)
            {
                throw new SubjectClassNotFoundException(Guid.Empty);
            }
            var subjectClass = await _repository.SubjectClassRepository.GetSubjectClassAsync(id, trackChanges);
            return subjectClass is null ? throw new SubjectClassNotFoundException(id ?? Guid.Empty) : subjectClass;
        }

        public async Task<SubjectTeacher> GetSubjectTeacherAndCheckIfItExists(Guid? id, bool trackChanges)
        {
            if (id == null)
            {
                throw new SubjectTeacherNotFoundException(Guid.Empty);
            }
            var subjectTeacher = await _repository.SubjectTeacherRepository.GetSubjectTeacher(id, trackChanges);
            return subjectTeacher is null ? throw new SubjectTeacherNotFoundException(id ?? Guid.Empty) : subjectTeacher;
        }

        public async Task<Teacher> GetTeacherAndCheckIfItExists(Guid? id, bool trackChanges)
        {
            if (id == null)
            {
                throw new TeacherNotFoundException(Guid.Empty);
            }
            var teacher = await _repository.TeacherRepository.GetTeacherAsync(id, trackChanges);
            return teacher is null ? throw new TeacherNotFoundException(id ?? Guid.Empty) : teacher;
        }
    }
}
