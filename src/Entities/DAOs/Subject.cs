using Entities.Common;

namespace Entities.DAOs
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;

        public ICollection<SubjectTeacher> SubjectTeachers { get; set; } = null!;
        public ICollection<SubjectClass> SubjectClasses { get; set; } = null!;
        public ICollection<Assignment> Assignments { get; set; } = null!;
    }
}
