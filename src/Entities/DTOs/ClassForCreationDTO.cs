using Entities.Common;

namespace Entities.DTOs
{
    public record ClassForCreationDTO(
        int Grade,
        ESchoolShift SchoolShift,
        string Name,
        int StartYear,
        int EndYear,
        int PeriodCount
    );
}
