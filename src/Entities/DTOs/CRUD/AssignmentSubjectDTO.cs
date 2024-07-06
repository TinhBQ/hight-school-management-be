namespace Entities.DTOs.CRUD
{
    public record AssignmentSubjectDTO
    {
        public SubjectDTO? Subject { get; set; }
        public IEnumerable<AssignmentClassTeacherDTO>? ClassTeachers { get; set; }

        public int? PeriodCount { get; set; }
    }
}
