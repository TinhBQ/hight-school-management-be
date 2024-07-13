using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record AssignmentForCreationDTO
    {
        public  int PeriodCount { get; set; }
        public  ESchoolShift SchoolShift { get; set; }
        public int Semester { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public Guid? TeacherId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
    }
}
