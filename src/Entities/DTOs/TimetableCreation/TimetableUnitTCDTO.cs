using Entities.Common;

namespace Entities.DTOs.TimetableCreation
{
    public record TimetableUnitTCDTO
    {
        public EPriority Priority { get; set; } = EPriority.None;
        public int StartAt { get; set; }
        public Guid AssignmentId { get; set; } = Guid.Empty;
        public AssignmentTCDTO Assignment { get; init; } = null!;

        public TimetableUnitTCDTO() { }

        public TimetableUnitTCDTO(AssignmentTCDTO Assignment)
        {
            this.AssignmentId = Assignment.Id;
            this.Assignment = Assignment;
        }
    }
}
