using Entities.Common;

namespace Entities.DAOs
{
    public class TimetableUnit : BaseEntity
    {
        public string TeacherName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public EPriority Priority { get; set; }
        public int StartAt { get; set; }

        public Guid? TeacherId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? SubjectId { get; set; }

        public Guid? TimetableId { get; set; }
        public Timetable Timetable { get; set; } = null!;
    }
}
