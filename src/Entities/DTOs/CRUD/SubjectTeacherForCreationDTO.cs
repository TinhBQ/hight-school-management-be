using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record SubjectTeacherForCreationDTO
        (
            bool IsMain,
            Guid SubjectId,
            Guid TeacherId
        );
}
