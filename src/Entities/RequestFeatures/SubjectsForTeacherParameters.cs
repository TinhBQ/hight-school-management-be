namespace Entities.RequestFeatures
{
    public class SubjectsForTeacherParameters : RequestParameters
    {

        public SubjectsForTeacherParameters() => OrderBy = "";

        public string? SearchTerm { get; set; }

        public Guid teacherId { get; set; }
    }
}
