namespace Entities.RequestFeatures
{
    public class SubjectTeacherParameters : RequestParameters
    {
        public SubjectTeacherParameters() => OrderBy = "";

        public string? SearchTerm { get; set; }

        public Guid? teacherId { get; set; }
    }
}
