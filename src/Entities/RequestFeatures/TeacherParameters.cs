namespace Entities.RequestFeatures
{
    public class TeacherParameters : RequestParameters
    {
        public TeacherParameters() => OrderBy = "name";

        public string? SearchTerm { get; set; }

        public bool? IsAssignedHomeroom { get; set; }

        public uint? StartYear { get; set; }

        public uint? EndYear { get; set; }
    }
}
