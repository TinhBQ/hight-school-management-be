namespace Entities.RequestFeatures
{
    public class TimetableReqParams : RequestParameters
    {
        public TimetableReqParams() => OrderBy = "";

        public string? SearchTerm { get; set; }

        public uint? StartYear { get; set; }

        public uint? EndYear { get; set; }

        public uint? Semester { get; set; }
    }
}
