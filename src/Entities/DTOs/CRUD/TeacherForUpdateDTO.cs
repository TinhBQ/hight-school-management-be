namespace Entities.DTOs.CRUD
{
    public record TeacherForUpdateDTO
        (
            string FirstName,
            string MiddleName,
            string LastName,
            string ShortName
        );
}
