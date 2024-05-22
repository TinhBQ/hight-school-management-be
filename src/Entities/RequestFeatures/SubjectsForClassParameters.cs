namespace Entities.RequestFeatures
{
    public class SubjectsForClassParameters : RequestParameters
    {

        public SubjectsForClassParameters() => OrderBy = "";

        public string? SearchTerm { get; set; }

        public Guid classId { get; set; }
    }
}
