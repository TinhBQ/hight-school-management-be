using Contexts;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager(HsmsDbContext hsmsDbContext) : IRepositoryManager
    {
        private readonly Lazy<IClassRepository> _classRepository = new(() => new ClassRepository(hsmsDbContext));
        private readonly Lazy<ISubjectRepository> _subjectRepository = new(() => new SubjectRepository(hsmsDbContext));
        private readonly Lazy<ITeacherRepository> _teacherRepository = new(() => new TeacherRepository(hsmsDbContext));

        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork = new(() => new UnitOfWork(hsmsDbContext));

        public IClassRepository ClassRepository => _classRepository.Value;
        public ISubjectRepository SubjectRepository => _subjectRepository.Value;
        public ITeacherRepository TeacherRepository => _teacherRepository.Value;


        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
