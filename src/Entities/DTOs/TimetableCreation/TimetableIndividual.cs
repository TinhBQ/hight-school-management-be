using Entities.Common;

namespace Entities.DTOs.TimetableCreation
{
    public class TimetableIndividual(
        ETimetableFlag[,] timetableFlag,
        List<TimetableUnitTCDTO> timetableUnits,
        List<ClassTCDTO> classes,
        List<TeacherTCDTO> teachers)
    {
        public ETimetableFlag[,] TimetableFlag { get; set; } = timetableFlag;
        public List<TimetableUnitTCDTO> TimetableUnits { get; set; } = timetableUnits;
        public List<ClassTCDTO> Classes { get; init; } = classes;
        public List<TeacherTCDTO> Teachers { get; init; } = teachers;
        public List<string> Errors { get; set; } = [];
        public int Adaptability { get; set; }
        public int Age { get; set; }
        public int Longevity { get; set; }
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
    }
}
