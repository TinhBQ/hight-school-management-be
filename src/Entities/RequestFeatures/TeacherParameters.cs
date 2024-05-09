namespace Entities.RequestFeatures
{
    public class TeacherParameters : RequestParameters
    {
        public TeacherParameters() => OrderBy = "name";

        public string? searchTerm { get; set; }
    }
}
