namespace Entities.DTOs
{
    public record TeacherForUpdateDTO
        (
            string FirstName,
            string MiddleName,
            string LastName,
            string ShortName
        );
}
