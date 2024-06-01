namespace Entities.RequestFeatures
{
    public class AssignmentParameters : RequestParameters
    {
        public AssignmentParameters() => OrderBy = "";

        public string? SearchTerm { get; set; }

        public uint? StartYear { get; set; }

        public uint? EndYear { get; set; }

        public uint? Semester { get; set; }
    }
}
