using Entities.Common;

namespace Entities.DAOs
{
    public class Assignment : BaseEntity
    {
        public int PeriodCount { get; set; }
        public ESchoolShift SchoolShift { get; set; }
        public int Semester { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public Guid? TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;
    }
}
