using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record ClassToHomeroomAssignmentForUpdateCollectionDTO(
        Guid Id,
        Guid HomeroomTeacherId
    );
}
