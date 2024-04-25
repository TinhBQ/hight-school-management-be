using Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DAOs
{
    public class TimetableUnit : BaseEntity
    {
        public EPriority Priority { get; set; }
        public int StartAt { get; set; }


        [ForeignKey(nameof(Timetable))]
        public Guid TimetableId { get; set; }
        public Timetable Timetable { get; set; } = null!;


        [ForeignKey(nameof(Assignment))]
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
    }
}
