using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record ClassForCreationDTO(
        int Grade,
        ESchoolShift SchoolShift,
        string Name,
        int StartYear,
        int EndYear
    );
}
