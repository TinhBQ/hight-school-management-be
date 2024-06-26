using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record SubjectTeacherDTO
        (
            Guid Id,
            bool IsMain,
            SubjectDTO? Subject,
            TeacherDTO? Teacher,
            DateTime CreateAt,
            DateTime UpdateAt
        );
}
