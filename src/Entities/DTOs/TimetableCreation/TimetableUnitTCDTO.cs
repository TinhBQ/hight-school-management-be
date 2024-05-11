using Entities.Common;

namespace Entities.DTOs.TimetableCreation
{
    public record TimetableUnitTCDTO
    {
        public Guid Id { get; init; }
        public EPriority Priority { get; set; } = EPriority.None;
        public string ClassName = null!;
        public string TeacherName = null!;
        public string SubjectName = null!;
        public int StartAt { get; set; }
        public Guid AssignmentId { get; set; } = Guid.Empty;
        public AssignmentTCDTO Assignment { get; init; } = null!;

        public TimetableUnitTCDTO() { }

        public TimetableUnitTCDTO(AssignmentTCDTO assignment)
        {
            Id = Guid.NewGuid();
            ClassName = assignment.Class.Name;
            TeacherName = assignment.Teacher.ShortName;
            SubjectName = assignment.Subject.ShortName;
            AssignmentId = assignment.Id;
            Assignment = assignment;
        }
    }
}
