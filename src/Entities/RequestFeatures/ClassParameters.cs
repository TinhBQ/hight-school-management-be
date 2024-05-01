namespace Entities.RequestFeatures
{
    public class ClassParameters : RequestParameters
    {
        public ClassParameters() => orderBy = "name";

        public string? searchTerm { get; set; }
    }
}
