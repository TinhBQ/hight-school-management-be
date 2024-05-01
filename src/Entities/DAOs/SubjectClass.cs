using Entities.Common;

namespace Entities.DAOs
{
    public class SubjectClass : BaseEntity
    {
        public int PeriodCount { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;
    }
}
