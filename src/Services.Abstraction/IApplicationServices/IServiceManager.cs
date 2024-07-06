namespace Services.Abstraction.IApplicationServices
{
    public interface IServiceManager
    {
        IClassService ClassService { get; }
        ISubjectService SubjectService { get; }
        ITeacherService TeacherService { get; }
        ISubjectClassService SubjectClassService { get; }
        ISubjectTeacherService SubjectTeacherService { get; }

        IAssignmentBQTService AssignmentBQTService { get; }
    }
}
