using Entities.Common;

namespace Entities.DTOs.TimetableCreation
{
    public record TimetableUnitTCDTO(AssignmentTCDTO assignment)
    {
        public EPriority Priority { get; set; } = EPriority.None;
        public int StartAt { get; set; }
        public AssignmentTCDTO Assignment { get; init; } = assignment;
    }
}
