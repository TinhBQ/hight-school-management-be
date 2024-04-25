using Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DAOs
{
    public class SubjectTeacher : BaseEntity
    {
        public bool IsMain { get; set; }


        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;


        [ForeignKey(nameof(Teacher))]
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
    }
}
