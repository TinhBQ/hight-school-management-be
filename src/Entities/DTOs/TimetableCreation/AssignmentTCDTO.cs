using Entities.Common;
using Entities.DAOs;

namespace Entities.DTOs.TimetableCreation
{
    public class AssignmentTCDTO
    {
        public Guid Id { get; init; } = Guid.Empty;
        public int PeriodCount { get; init; }
        public ESchoolShift SchoolShift { get; init; } = ESchoolShift.Morning;
        public TeacherTCDTO Teacher { get; init; } = null!;
        public SubjectTCDTO Subject { get; init; } = null!;
        public ClassTCDTO Class { get; init; } = null!;

        public AssignmentTCDTO(ClassTCDTO @class)
        {
            SchoolShift = (ESchoolShift)@class.SchoolShift!;
            Class = @class;
        }

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
