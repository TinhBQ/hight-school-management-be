namespace Entities.DTOs.TimetableCreation
{
    public class ConstraintError
    {
        public string Code { get; set; } = string.Empty;
        public bool IsHardConstraint { get; set; } = true;
        public string TeacherName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Age { get; set; }

        public ConstraintError() { }

        public ConstraintError(string code, bool isHardConstraint, string teacherName, string className, string subjectName, string description)
        {
            Code = code;
            IsHardConstraint = isHardConstraint;
            TeacherName = teacherName;
            ClassName = className;
            SubjectName = subjectName;
            Description = description;
        }
    }
}
