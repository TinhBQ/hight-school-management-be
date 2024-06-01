using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record ClassToHomeroomAssignmentDTO(
        Guid Id,
        string Name,
        TeacherDTO? HomeroomTeacher,
        DateTime CreateAt,
        DateTime UpdateAt
    );
}
