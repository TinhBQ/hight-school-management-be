using Entities.DAOs;
using Entities.DTOs.TimetableCreation;

namespace Entities.RequestFeatures
{
    public class TimetableParameters
    {
        public List<Guid> ClassIds { get; set; } = [];
        public List<Subject> DoublePeriodSubjects { get; set; } = [];
        public List<TimetableUnitTCDTO> FixedTimetableUnits { get; set; } = [];
        public List<TimetableUnitTCDTO> FreeTimetableUnits { get; set; } = []; // Ds tiết trống - dùng cho kiểm tra tiết lủng // bỏ
        public List<TimetableUnitTCDTO> NoAssignTimetableUnits { get; set; } = []; // Ds tiết không xếp
        public List<TimetableUnitTCDTO> BusyTimetableUnits { get; set; } = []; // Ds tiết bận - dùng cho kiểm tra lịch bận của gv
        public List<(Subject Subject, int RoomCount)> SubjectsWithPracticeRoom { get; set; } = [];
        public int MaxPeriodPerDay { get; set; } = 5;
        public int MinPeriodPerDay { get; set; } = 0;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }
    }
}
