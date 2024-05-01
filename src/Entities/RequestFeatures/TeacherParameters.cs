namespace Entities.RequestFeatures
{
    public class TeacherParameters : RequestParameters
    {
        public TeacherParameters() => orderBy = "name";

        public string? searchTerm { get; set; }
    }
}
