using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record AssignmentDTO(
        Guid Id,
        int PeriodCount,
        ESchoolShift SchoolShift,
        int Semester,
        int StartYear,
        int EndYear,
        Guid TeacherId,
        Guid SubjectId,
        Guid ClassId,
        DateTime CreateAt,
        DateTime UpdateAt
        );
}
