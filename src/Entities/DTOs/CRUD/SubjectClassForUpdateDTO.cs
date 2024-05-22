namespace Entities.DTOs.CRUD
{
    public record SubjectClassForUpdateDTO
        (
            Guid SubjectId,
            Guid ClassId,
            int PeriodCount
        );
}
