using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public class ClassTCDTO(Class @class)
    {
        public Guid Id { get; init; } = @class.Id;
        public string Name { get; init; } = @class.Name;
        public int Grade { get; init; } = @class.Grade;
        public ESchoolShift? SchoolShift { get; init; } = @class.SchoolShift;
        public int PeriodCount { get; init; } = @class.PeriodCount;
        public Guid? HomeroomTeacherId { get; init; } = @class.HomeroomTeacherId;
    }
}
