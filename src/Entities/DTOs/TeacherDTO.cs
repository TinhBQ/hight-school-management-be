namespace Entities.DTOs
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
