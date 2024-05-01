namespace Entities.DTOs
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
