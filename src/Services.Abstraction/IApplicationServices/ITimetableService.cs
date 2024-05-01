using Entities.DAOs;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITimetableService
    {
        public Timetable Create(
            List<Guid> classIds,
            /* List<String> doublePeriodSubjectIds,*/
            List<string> doublePeriodSubjectShortName,
            List<TimetableUnit> fixedTimetableUnits,
            List<TimetableUnit> busyUnits,
            int maxEmptyPeriodCount,
            List<(Guid, int, int)> maxMinPeriodCount,
            bool?[,] timetableFlag,
            int startYear,
            int endYear,
            int semester);

        public Timetable Get(string id);

        public void Update(Timetable timetable);

        public void Delete(string id);
    }
}
