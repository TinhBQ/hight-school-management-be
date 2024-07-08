using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record ClassDTO
    {
        public Guid Id { get; set; }
        public int Grade { get; set; }
        public ESchoolShift SchoolShift { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int PeriodCount { get; set; }
        public TeacherDTO HomeroomTeacher { get; set; } = null!;

        public IEnumerable<SubjectClassDTO>? subjectClasses { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    };
}
