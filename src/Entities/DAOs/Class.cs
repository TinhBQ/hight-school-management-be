
using Entities.Common;

namespace Entities.DAOs
{
    public class Class : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int Grade { get; set; }
        public ESchoolShift SchoolShift { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int PeriodCount { get; set; }

        public Guid? HomeroomTeacherId { get; set; }
        public Teacher HomeroomTeacher { get; set; } = null!;

        public ICollection<Assignment> Assignments { get; set; } = null!;
        public ICollection<SubjectClass> SubjectClasses { get; set; } = null!;
    }
}
