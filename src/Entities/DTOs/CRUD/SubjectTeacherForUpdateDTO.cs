namespace Entities.DTOs.CRUD
{
    public record SubjectTeacherForUpdateDTO
        (
            Guid SubjectId,
            Guid TeacherId,
            bool IsMain
        );
}
