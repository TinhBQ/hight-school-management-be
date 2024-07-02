using Entities.Common;

namespace Entities.DTOs.TimetableCreation
{
    public record TimetableUnitTCDTO
    {
        public Guid Id { get; init; }
        public EPriority Priority { get; set; } = EPriority.None;
        public Guid ClassId { get; init; }
        public string ClassName { get; init; } = string.Empty;
        public Guid TeacherId { get; init; }
        public string TeacherName { get; init; } = string.Empty;
        public Guid SubjectId { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public int StartAt { get; set; }
        public Guid AssignmentId { get; set; } = Guid.Empty;
        public List<ConstraintError> ConstraintErrors { get; set; } = [];

        public TimetableUnitTCDTO() { }

        public TimetableUnitTCDTO(AssignmentTCDTO assignment)
        {
            Id = Guid.NewGuid();
            ClassId = assignment.Class.Id;
            ClassName = assignment.Class.Name;
            TeacherId = assignment.Teacher.Id;
            TeacherName = assignment.Teacher.ShortName;
            SubjectId = assignment.Subject.Id;
            SubjectName = assignment.Subject.ShortName;
            AssignmentId = assignment.Id;
        }
    }
}
