using AutoMapper;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;

namespace Services.Implementation
{
    public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper) : IServiceManager
    {
        private readonly Lazy<IClassService> _classService = new(() => new ClassService(repositoryManager, logger, mapper));
        private readonly Lazy<ISubjectService> _subjectService = new(() => new SubjectService(repositoryManager, logger, mapper));
        private readonly Lazy<ITeacherService> _teacherService = new(() => new TeacherService(repositoryManager, logger, mapper));

        private readonly Lazy<ISubjectClassService> _subjectClassService = new(() => new SubjectClassService(repositoryManager, logger, mapper));

        public IClassService ClassService => _classService.Value;

        public ISubjectService SubjectService => _subjectService.Value;

        public ITeacherService TeacherService => _teacherService.Value;

        public ISubjectClassService SubjectClassService => _subjectClassService.Value;
    }
}
