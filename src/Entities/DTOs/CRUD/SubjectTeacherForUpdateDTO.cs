namespace Entities.DTOs.CRUD
{
    public record SubjectTeacherForUpdateDTO
        (
            Guid SubjectId,
            Guid ClassId,
            int PeriodCount
        );
}
