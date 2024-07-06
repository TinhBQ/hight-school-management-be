using Entities.Common;

namespace Entities.DTOs.CRUD
{
    public record AssignmentClassTeacherDTO
    {
        public ClassDTO? Class { get; set; }
        public TeacherDTO? Teacher { get; set; }
    }
}