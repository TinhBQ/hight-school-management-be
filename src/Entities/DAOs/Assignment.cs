using Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DAOs
{
    public class Assignment : BaseEntity
    {
        public int PeriodCount { get; set; }
        public ESchoolShift SchoolShift { get; set; }
        public int Semester { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }


        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;


        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;


        [ForeignKey(nameof(Class))]
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;


        public ICollection<TimetableUnit> TimetableUnits { get; set; } = null!;
    }
}
