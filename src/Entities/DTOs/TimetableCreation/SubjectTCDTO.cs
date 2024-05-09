using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public class SubjectTCDTO(Subject subject)
    {
        public Guid Id { get; init; } = subject.Id;
        public string Name { get; init; } = subject.Name;
        public string ShortName { get; init; } = subject.ShortName;
    }
}
