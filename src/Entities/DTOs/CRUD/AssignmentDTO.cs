using Entities.Common;

namespace Entities.DTOs.CRUD
{
    //public record AssignmentDTO(
    //    Guid Id,
    //    int PeriodCount,
    //    ESchoolShift SchoolShift,
    //    int Semester,
    //    int StartYear,
    //    int EndYear,
    //    Guid TeacherId,
    //    string TeacherName,
    //    string TeacherShortName,
    //    Guid SubjectId,
    //    string SubjectName,
    //    Guid ClassId,
    //    string ClassName
    //    );

    public record AssignmentDTO
    {
        public Guid Id { get; set; }
        public int PeriodCount { get; set; }
        public ESchoolShift SchoolShift { get; set; }
        public int Semester { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherShortName { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
    }
}
