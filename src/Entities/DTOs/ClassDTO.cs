using Entities.Common;

namespace Entities.DTOs
{
    public record ClassDTO(
        Guid Id,
        int Grade,
        ESchoolShift SchoolShift,
        string Name,
        int StartYear,
        int EndYear,
        int PeriodCount,
        DateTime CreateAt,
        DateTime UpdateAt
    );
}
