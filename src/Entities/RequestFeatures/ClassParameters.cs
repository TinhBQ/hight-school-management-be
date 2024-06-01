namespace Entities.RequestFeatures
{
    public class ClassParameters : RequestParameters
    {
        public ClassParameters() => OrderBy = "name";

        public string? SearchTerm { get; set; }

        public uint? StartYear { get; set; }

        public uint? EndYear { get; set; }

        public uint? Grade { get; set; }

        public bool? IsAssignedHomeroom { get; set; }
    }
}
