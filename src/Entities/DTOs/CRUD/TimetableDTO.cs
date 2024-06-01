using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record TimetableDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }
        public ICollection<TimetableUnit> TimetableUnits { get; set; } = null!;
    }
}
