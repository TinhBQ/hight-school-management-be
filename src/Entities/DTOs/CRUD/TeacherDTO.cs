namespace Entities.DTOs.CRUD
{
    public record TeacherDTO
        (
            Guid Id,
            string FirstName,
            string MiddleName,
            string LastName,
            string ShortName,
            DateTime CreateAt,
            DateTime UpdateAt
        );
}
