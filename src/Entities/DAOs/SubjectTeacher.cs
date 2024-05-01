using Entities.Common;

namespace Entities.DAOs
{
    public class SubjectTeacher : BaseEntity
    {
        public bool IsMain { get; set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
    }
}
