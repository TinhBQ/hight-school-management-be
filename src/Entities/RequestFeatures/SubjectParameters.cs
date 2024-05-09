namespace Entities.RequestFeatures
{
    public class SubjectParameters : RequestParameters
    {
        public SubjectParameters() => OrderBy = "name";

        public string? searchTerm { get; set; }
    }
}
