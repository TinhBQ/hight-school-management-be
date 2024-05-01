namespace Entities.RequestFeatures
{
    public class SubjectParameters : RequestParameters
    {
        public SubjectParameters() => orderBy = "name";

        public string? searchTerm { get; set; }
    }
}
