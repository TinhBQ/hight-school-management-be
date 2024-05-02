namespace Entities.DTOs.CRUD
{
    public record TeacherForCreationDTO
        (
            string FirstName,
            string MiddleName,
            string LastName,
            string ShortName
        );
}
