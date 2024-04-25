using AutoMapper;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IClassService> _classService;
        private readonly Lazy<ISubjectService> _subjectService;
        private readonly Lazy<ITeacherService> _teacherService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _classService = new Lazy<IClassService>(() => new ClassService(repositoryManager, logger, mapper));
            _subjectService = new Lazy<ISubjectService>(() => new SubjectService(repositoryManager, logger, mapper));
            _teacherService = new Lazy<ITeacherService>(() => new TeacherService(repositoryManager, logger, mapper));
        }

        public IClassService ClassService => _classService.Value;

        public ISubjectService SubjectService => _subjectService.Value;

        public ITeacherService TeacherService => _teacherService.Value;
    }
}
