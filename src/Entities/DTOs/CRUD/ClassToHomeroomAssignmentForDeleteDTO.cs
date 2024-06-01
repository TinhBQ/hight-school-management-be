using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record ClassToHomeroomAssignmentForDeleteDTO
    {
        public Guid? HomeroomTeacherId { get; } = null;
    }
}
