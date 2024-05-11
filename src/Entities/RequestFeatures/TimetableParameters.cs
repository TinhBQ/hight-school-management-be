using Entities.DTOs.TimetableCreation;

namespace Entities.RequestFeatures
{
    public class TimetableParameters
    {
        public List<Guid> ClassIds { get; set; } = [];
        public List<Guid> DoublePeriodSubjectIds { get; set; } = [];
        public List<TimetableUnitTCDTO> FixedTimetableUnits { get; set; } = [];
        public List<TimetableUnitTCDTO> FreeTimetableUnits { get; set; } = [];
        public List<TimetableUnitTCDTO> BusyTimetableUnits { get; set; } = [];
        public int MaxPeriodPerDay { get; set; } = 5;
        public int MinPeriodPerDay { get; set; } = 0;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }
    }
}
