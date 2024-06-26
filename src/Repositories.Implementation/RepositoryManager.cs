using Contexts;
using Repositories.Implementation;
using Services.Abstraction.IRepositoryServices;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager(HsmsDbContext hsmsDbContext) : IRepositoryManager
    {
        private readonly Lazy<IAssignmentRepository> _assignmentRepository = new(() => new AssignmentRepository(hsmsDbContext));
        private readonly Lazy<IClassRepository> _classRepository = new(() => new ClassRepository(hsmsDbContext));
        private readonly Lazy<ISubjectRepository> _subjectRepository = new(() => new SubjectRepository(hsmsDbContext));
        private readonly Lazy<ITeacherRepository> _teacherRepository = new(() => new TeacherRepository(hsmsDbContext));
        private readonly Lazy<ISubjectClassRepository> _subjectClassRepository = new(() => new SubjectClassRepository(hsmsDbContext));
        private readonly Lazy<ISubjectTeacherRepository> _subjectTeacherRepository = new(() => new SubjectTeacherRepository(hsmsDbContext));


        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork = new(() => new UnitOfWork(hsmsDbContext));

        public IAssignmentRepository AssignmentRepository => _assignmentRepository.Value;
        public IClassRepository ClassRepository => _classRepository.Value;
        public ISubjectRepository SubjectRepository => _subjectRepository.Value;
        public ITeacherRepository TeacherRepository => _teacherRepository.Value;
        public ISubjectClassRepository SubjectClassRepository => _subjectClassRepository.Value;
        public ISubjectTeacherRepository SubjectTeacherRepository => _subjectTeacherRepository.Value;


        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
