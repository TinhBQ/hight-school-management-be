using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITimetableService
    {
        public Timetable Create(TimetableParameters parameters);

        public Timetable Get(string id);

        public void Update(Timetable timetable);

        public void Delete(string id);
    }
}
