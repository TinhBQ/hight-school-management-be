namespace Entities.DTOs.CRUD
{
    public record SubjectClassForCreationDTO
        (
            Guid SubjectId,
            Guid ClassId,
            int PeriodCount
        );
}
