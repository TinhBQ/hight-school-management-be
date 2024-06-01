using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record ClassDTO(
        Guid Id,
        int Grade,
        ESchoolShift SchoolShift,
        string Name,
        int StartYear,
        int EndYear,
        int PeriodCount,
        TeacherDTO HomeroomTeacher,
        DateTime CreateAt,
        DateTime UpdateAt
    );
}
