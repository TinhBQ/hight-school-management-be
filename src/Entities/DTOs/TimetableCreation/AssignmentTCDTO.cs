using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public record AssignmentTCDTO
    {
        public Guid Id { get; init; }
        public int PeriodCount { get; init; }
        public ESchoolShift SchoolShift { get; init; }
        public TeacherTCDTO Teacher { get; init; }
        public SubjectTCDTO Subject { get; init; }
        public ClassTCDTO Class { get; init; }

        public AssignmentTCDTO(Assignment assignment, TeacherTCDTO teacher, ClassTCDTO @class, SubjectTCDTO subject)
        {
            Id = assignment.Id;
            PeriodCount = assignment.PeriodCount;
            SchoolShift = assignment.SchoolShift;
            Teacher = teacher;
            Subject = subject;
            Class = @class;
        }
    }
}
