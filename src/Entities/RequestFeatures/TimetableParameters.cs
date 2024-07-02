using Entities.DTOs.TimetableCreation;

namespace Entities.RequestFeatures
{
    public class TimetableParameters
    {
        public List<Guid> ClassIds { get; set; } = [];
        public List<SubjectTCDTO> DoublePeriodSubjects { get; set; } = [];
        public List<TimetableUnitTCDTO> FixedTimetableUnits { get; set; } = [];
        public List<TimetableUnitTCDTO> FreeTimetableUnits { get; set; } = []; // Ds tiết trống - dùng cho kiểm tra tiết lủng
        public List<TimetableUnitTCDTO> NoAssignTimetableUnits { get; set; } = []; // Ds tiết không xếp
        public List<TimetableUnitTCDTO> BusyTimetableUnits { get; set; } = []; // Ds tiết bận - dùng cho kiểm tra lịch bận của gv
        public Dictionary<Guid, int> SubjectsWithPracticeRoom { get; set; } = [];
        public int MaxPeriodPerDay { get; set; } = 5;
        public int MinPeriodPerDay { get; set; } = 0;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }
    }
}
