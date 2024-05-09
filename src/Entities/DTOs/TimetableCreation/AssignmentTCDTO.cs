using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public class AssignmentTCDTO(Assignment assignment, TeacherTCDTO teacher, ClassTCDTO @class, SubjectTCDTO subject)
    {
        public Guid Id { get; init; } = assignment.Id;
        public int PeriodCount { get; init; } = assignment.PeriodCount;
        public ESchoolShift SchoolShift { get; init; } = assignment.SchoolShift;
        public TeacherTCDTO Teacher { get; init; } = teacher;
        public SubjectTCDTO Subject { get; init; } = subject;
        public ClassTCDTO Class { get; init; } = @class;
    }
}
