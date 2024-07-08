namespace Entities.RequestFeatures
{
    public class AssignmentParameters : RequestParameters
    {
        public AssignmentParameters() => OrderBy = "";

        public string? SearchTerm { get; set; }

        public uint? StartYear { get; set; }

        public uint? EndYear { get; set; }

        public uint? Semester { get; set; }

        public Guid? ClassId { get; set; }

        public Guid? TeacherId { get; set; }

        public Guid? SubjectId { get; set; }

        public bool? IsNotAssigned { get; set; }
    }
}
