using Entities.DTOs.TimetableCreation;

namespace Entities.DTOs.CRUD
{
    public record TimetableDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }
        public List<TimetableUnitTCDTO> TimetableUnits { get; set; } = [];
        public List<ConstraintError> ConstraintErrors { get; set; } = [];
        public List<ClassTCDTO> Classes { get; set; } = [];
        public List<TeacherTCDTO> Teachers { get; set; } = [];
    }
}
