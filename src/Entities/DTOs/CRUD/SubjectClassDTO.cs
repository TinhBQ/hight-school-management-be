using Entities.DAOs;

namespace Entities.DTOs.CRUD
{
    public record SubjectClassDTO
        (
            Guid Id,
            int PeriodCount,
            SubjectDTO Subject,
            ClassDTO Class,
            DateTime CreateAt,
            DateTime UpdateAt
        );
}
