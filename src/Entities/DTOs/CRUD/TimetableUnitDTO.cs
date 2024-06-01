using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record TimetableUnitDTO
    {
        public Guid Id { get; set; }
        public EPriority Priority { get; set; }
        public int StartAt { get; set; }

        public required Teacher Teacher { get; set; }
        public required Class Class { get; set; }
        public required Subject Subject { get; set; }
        public required Timetable Timetable { get; set; } = null!;
    }
}
