using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record AssignmentForUpdateDTO(
        Guid Id,
        int PeriodCount,
        ESchoolShift SchoolShift,
        int Semester,
        int StartYear,
        int EndYear,
        Guid TeacherId,
        Guid SubjectId,
        Guid ClassId
        );
}
