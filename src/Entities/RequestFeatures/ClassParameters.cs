namespace Entities.RequestFeatures
{
    public class ClassParameters : RequestParameters
    {
        public ClassParameters() => OrderBy = "name";

        public string? SearchTerm { get; set; }
    }
}
