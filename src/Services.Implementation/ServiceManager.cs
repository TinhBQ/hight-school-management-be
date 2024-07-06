using AutoMapper;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHelperService helperService) : IServiceManager
    {
        private readonly Lazy<IClassService> _classService = new(() => new ClassService(repositoryManager, logger, mapper, helperService));
        private readonly Lazy<ISubjectService> _subjectService = new(() => new SubjectService(repositoryManager, logger, mapper, helperService));
        private readonly Lazy<ITeacherService> _teacherService = new(() => new TeacherService(repositoryManager, logger, mapper, helperService));
        private readonly Lazy<ISubjectClassService> _subjectClassService = new(() => new SubjectClassService(repositoryManager, logger, mapper, helperService));
        private readonly Lazy<ISubjectTeacherService> _subjectTeacherService = new(() => new SubjectTeacherService(repositoryManager, logger, mapper, helperService));
        private readonly Lazy<IAssignmentBQTService> _assignmentBQTService = new(() => new AssignmentBQTService(repositoryManager, logger, mapper, helperService));

        public IClassService ClassService => _classService.Value;

        public ISubjectService SubjectService => _subjectService.Value;

        public ITeacherService TeacherService => _teacherService.Value;

        public ISubjectClassService SubjectClassService => _subjectClassService.Value;

        public ISubjectTeacherService SubjectTeacherService => _subjectTeacherService.Value;

        public IAssignmentBQTService AssignmentBQTService => _assignmentBQTService.Value;
    }
}
