using Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DAOs
{
    public class SubjectClass : BaseEntity
    {
        public int PeriodCount { get; set; }


        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;


        [ForeignKey(nameof(Class))]
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;
    }
}
