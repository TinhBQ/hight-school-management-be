using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record TeacherDTO
    {
        public Guid Id { get; init; }
        public string? FullName { get; init; }
        public string? ShortName { get; init; }
        public int? PeriodCount { get; set; }
        public IEnumerable<SubjectTeacherDTO?>? SubjectTeachers { get; init; }
        public DateTime CreateAt { get; init; }
        public DateTime UpdateAt { get; init; }
    }
}
