using Entities.Common;
using System.Runtime.Serialization;

namespace Entities.DAOs
{
    public class Timetable : BaseEntity
    {
        public string Name { get; set; } = null!;

        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }

        [IgnoreDataMember]
        public string Parameters { get; set; } = null!;

        public ICollection<TimetableUnit> TimetableUnits { get; set; } = null!;
    }
}
