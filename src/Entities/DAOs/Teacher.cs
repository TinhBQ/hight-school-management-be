using Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DAOs
{
    public class Teacher : BaseEntity
    {
        public string? Username { get; set; }
        public string? Hash { get; set; }
        public string? Salt { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int PeriodCount { get; set; }


        [ForeignKey(nameof(Class))]
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;


        public ICollection<Assignment> Assignments { get; set; } = null!;
        public ICollection<SubjectTeacher> SubjectTeachers { get; set; } = null!;
    }
}
