using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public class TeacherTCDTO(Teacher teacher)
    {
        public Guid Id { get; init; } = teacher.Id;
        public string ShortName { get; init; } = teacher.ShortName;
        public int PeriodCount { get; init; } = teacher.PeriodCount;
        public Guid? ClassId { get; init; } = teacher.ClassId;
    }
}
