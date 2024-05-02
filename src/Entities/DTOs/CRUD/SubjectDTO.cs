namespace Entities.DTOs.CRUD
{
    public record SubjectDTO
        (
            Guid Id,
            string Name,
            string ShortName,
            DateTime CreateAt,
            DateTime UpdateAt
        );
}
