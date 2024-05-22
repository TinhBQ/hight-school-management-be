namespace Entities.RequestFeatures
{
    public class SubjectClassParameters : RequestParameters
    {
        public SubjectClassParameters() => OrderBy = "";

        public string? SearchTerm { get; set; }
    }
}
