using Entities.Common;

namespace Entities.DAOs
{
    public class Timetable : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<TimetableUnit> TimetableUnits { get; set; } = null!;
    }
}
