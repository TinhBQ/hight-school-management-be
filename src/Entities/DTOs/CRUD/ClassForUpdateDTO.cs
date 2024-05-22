using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record ClassForUpdateDTO(
        int Grade,
        ESchoolShift SchoolShift,
        string Name,
        int StartYear,
        int EndYear
    );
}
