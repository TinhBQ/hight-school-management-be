using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public record SubjectTCDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string ShortName { get; init; }

        public SubjectTCDTO(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            ShortName = subject.ShortName;
        }
    }
}
