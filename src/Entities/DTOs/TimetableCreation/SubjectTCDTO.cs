using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public class SubjectTCDTO
    {
        public Guid Id { get; init; } = Guid.Empty;
        public string Name { get; init; } = null!;
        public string ShortName { get; init; } = null!;

        public SubjectTCDTO() { }

        public SubjectTCDTO(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            ShortName = subject.ShortName;
        }
    }
}
