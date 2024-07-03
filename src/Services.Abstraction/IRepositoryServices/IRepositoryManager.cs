namespace Services.Abstraction.IRepositoryServices
{
    public interface IRepositoryManager
    {
        IAssignmentRepository AssignmentRepository { get; }

        IClassRepository ClassRepository { get; }

        ISubjectRepository SubjectRepository { get; }

        ITeacherRepository TeacherRepository { get; }

        ISubjectClassRepository SubjectClassRepository { get; }

        ISubjectTeacherRepository SubjectTeacherRepository { get; }

        IUnitOfWork UnitOfWork { get; }
    }
}
