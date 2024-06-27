using Entities.Common;

namespace Entities.DTOs.TimetableCreation
{
    public class TimetableIndividual
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ETimetableFlag[,] TimetableFlag { get; set; } = null!;
        public List<TimetableUnitTCDTO> TimetableUnits { get; set; } = [];
        public List<ClassTCDTO> Classes { get; init; } = [];
        public List<TeacherTCDTO> Teachers { get; init; } = [];
        public List<ConstraintError> ConstraintErrors { get; set; } = [];
        public int Adaptability { get; set; }
        public int Age { get; set; }
        public int Longevity { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Semester { get; set; }
        public string Name { get; set; } = string.Empty;

        public TimetableIndividual() { }

        public TimetableIndividual(
        ETimetableFlag[,] timetableFlag,
        List<TimetableUnitTCDTO> timetableUnits,
        List<ClassTCDTO> classes,
        List<TeacherTCDTO> teachers)
        {
            TimetableFlag = timetableFlag;
            TimetableUnits = timetableUnits;
            Classes = classes;
            Teachers = teachers;
        }

        public void GetConstraintErrors()
        {
            if (TimetableUnits.Count == 0)
                return;

            foreach (var u in TimetableUnits)
                ConstraintErrors.AddRange(u.ConstraintErrors);

            ConstraintErrors = [.. ConstraintErrors
                .DistinctBy(x => new { x.SubjectName, x.TeacherName, x.ClassName, x.Code })
                .OrderBy(x => x.Age)];
        }
    }

    public class TimetableRootIndividual(
        ETimetableFlag[,] timetableFlag,
        List<TimetableUnitTCDTO> timetableUnits,
        List<ClassTCDTO> classes,
        List<TeacherTCDTO> teachers)
        : TimetableIndividual(timetableFlag, timetableUnits, classes, teachers)
    {
        public new ETimetableFlag[,] TimetableFlag { get; init; } = timetableFlag;
        public new List<TimetableUnitTCDTO> TimetableUnits { get; init; } = [.. timetableUnits.OrderBy(u => u.ClassName)];
        public new List<ClassTCDTO> Classes { get; init; } = [.. classes.OrderBy(c => c.Name)];
        public new List<TeacherTCDTO> Teachers { get; init; } = [.. teachers.OrderBy(t => t.ShortName)];
        public new int Adaptability { get; init; }
        public new int Age { get; init; }
        public new int Longevity { get; init; }
        public new List<ConstraintError> ConstraintErrors { get; init; } = [];
    }
}
