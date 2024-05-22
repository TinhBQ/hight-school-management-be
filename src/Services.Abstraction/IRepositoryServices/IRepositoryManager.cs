namespace Services.Abstraction.IRepositoryServices
{
    public interface IRepositoryManager
    {
        IClassRepository ClassRepository { get; }

        ISubjectRepository SubjectRepository { get; }

        ITeacherRepository TeacherRepository { get; }

        ISubjectClassRepository SubjectClassRepository { get; }

        IUnitOfWork UnitOfWork { get; }
    }
}
