using Entities.Common;
using Entities.RequestFeatures;

namespace Entities.DTOs.TimetableCreation
{
    public class TimetableIndividual(
        ETimetableFlag[,] timetableFlag,
        List<TimetableUnitTCDTO> timetableUnits,
        List<ClassTCDTO> classes,
        List<TeacherTCDTO> teachers)
    {
        public ETimetableFlag[,] TimetableFlag { get; set; } = timetableFlag;
        public List<TimetableUnitTCDTO> TimetableUnits { get; set; } = [.. timetableUnits.OrderBy(u => u.ClassName)];
        public List<ClassTCDTO> Classes { get; init; } = [.. classes.OrderBy(c => c.Name)];
        public List<TeacherTCDTO> Teachers { get; init; } = [.. teachers.OrderBy(t => t.ShortName)];
        public int Adaptability { get; set; }

        public void UpdateAdaptability(TimetableParameters parameters)
        {
            CheckH01();
            CheckH02();
            CheckH03(parameters);
            CheckH04();
            CheckH05();
            CheckH06(parameters);
            CheckH07();
            CheckH08();
            CheckH09(parameters);
            CheckH10();
            CheckH11();
        }

        private void CheckH01()
        {
            foreach (var @class in Classes)
            {
                var timetableUnits = TimetableUnits.Where(u => u.Assignment.Class.Id == @class.Id).ToList();
                var uniqueUnitCount = timetableUnits.Select(u => u.StartAt).Distinct().Count();
                Adaptability += (timetableUnits.Count - uniqueUnitCount) * 1000;
            }
        }

        private void CheckH02()
        {
            foreach (var unit in TimetableUnits)
                if (unit.StartAt == 0)
                    Adaptability += 1000;
        }

        private void CheckH03(TimetableParameters parameters)
        {
            var fixedUnits = TimetableUnits.Where(u => u.Priority == EPriority.Fixed).ToList();
            if (fixedUnits.Count != parameters.FixedTimetableUnits.Count) throw new Exception();
            foreach (var unit in fixedUnits)
                if (!parameters.FixedTimetableUnits.Any(u => u.AssignmentId == unit.AssignmentId && u.StartAt == unit.StartAt))
                    Adaptability += 1000;
        }

        private void CheckH04() { }

        private void CheckH05()
        {
            foreach (var teacher in Teachers)
            {
                var timetableUnits = TimetableUnits.Where(u => u.Assignment.Teacher.Id == teacher.Id).ToList();
                var duplicatedUnitCount = timetableUnits.Select(u => u.StartAt).Distinct().Count();
                Adaptability += (timetableUnits.Count - duplicatedUnitCount) * 1000;
            }
        }

        private void CheckH06(TimetableParameters parameters)
        {
            foreach (var @class in Classes)
            {
                var doublePeriodUnits = new List<TimetableUnitTCDTO>();
                foreach (var subjectId in parameters.DoublePeriodSubjectIds)
                {
                    var result = false;
                    var timetableUnits = TimetableUnits
                        .Where(u => u.Assignment.Class.Id == @class.Id && u.Assignment.Subject.Id == subjectId)
                        .OrderBy(u => u.StartAt)
                        .ToList();
                    doublePeriodUnits.AddRange(timetableUnits);
                    for (var i = 0; i < timetableUnits.Count - 1; i++)
                        if (timetableUnits[i].StartAt == timetableUnits[i].StartAt - 1)
                        {
                            result = true;
                            break;
                        }
                    Adaptability += result ? 0 : 1000;
                }
            }
        }

        private void CheckH07() { }

        private void CheckH08() { }

        private void CheckH09(TimetableParameters parameters)
        {
            foreach (var @class in Classes)
            {
                var doublePeriodUnits = new List<TimetableUnitTCDTO>();
                foreach (var subjectId in parameters.DoublePeriodSubjectIds)
                    doublePeriodUnits.AddRange([.. TimetableUnits
                        .Where(u => u.Assignment.Class.Id == @class.Id && u.Assignment.Subject.Id == subjectId)
                        .OrderBy(u => u.StartAt)]);

                var singlePeriodUnits = TimetableUnits
                    .Where(u => u.ClassName == @class.Name && !doublePeriodUnits.Contains(u))
                    .OrderBy(u => u.StartAt)
                    .ToList();
                for (var i = 0; i < singlePeriodUnits.Count - 1; i++)
                    if (singlePeriodUnits[i].StartAt == singlePeriodUnits[i + 1].StartAt - 1 &&
                        singlePeriodUnits[i].SubjectName == singlePeriodUnits[i + 1].SubjectName)
                        Adaptability += 1000;
            }
        }

        private void CheckH10() { }

        private void CheckH11()
        {
            foreach (var teacher in teachers)
            {
                var timetableUnits = TimetableUnits
                    .Where(u => u.Assignment.Teacher.Id == teacher.Id)
                    .OrderBy(u => u.StartAt)
                    .ToList();
                var count = 0;
                for (var j = 1; j < 61; j += 10)
                    if (timetableUnits.Any(u => u.StartAt >= j && u.StartAt <= j + 9))
                        count++;
                if (7 - count < 2)
                    Adaptability += 1000;
            }
        }
    }
}
