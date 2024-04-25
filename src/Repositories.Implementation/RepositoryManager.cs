using Contexts;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IClassRepository> _classRepository;
        private readonly Lazy<ISubjectRepository> _subjectRepository;
        private readonly Lazy<ITeacherRepository> _teacherRepository;

        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(HsmsDbContext hsmsDbContext)
        {
            _classRepository = new Lazy<IClassRepository>(() => new ClassRepository(hsmsDbContext));
            _subjectRepository = new Lazy<ISubjectRepository>(() => new SubjectRepository(hsmsDbContext));
            _teacherRepository = new Lazy<ITeacherRepository>(() => new TeacherRepository(hsmsDbContext));

            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(hsmsDbContext));

        }


        public IClassRepository ClassRepository => _classRepository.Value;
        public ISubjectRepository SubjectRepository => _subjectRepository.Value;
        public ITeacherRepository TeacherRepository => _teacherRepository.Value;


        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
