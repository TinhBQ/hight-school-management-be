using Entities.Common;
using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITimetableServiceTemp
    {
        public Timetable Create(TimetableParameters tParameters, TimetableCreatorParameters tcParameters);

        public Timetable CreateDemo(TimetableParameters tParameters, TimetableCreatorParameters tcParameters);

        public Timetable Get(string id);

        public void Update(Timetable timetable);

        public void Delete(string id);
    }
}
