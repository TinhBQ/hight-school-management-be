using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITimetableService
    {
        public TimetableDTO Generate(TimetableParameters parameters);
        public TimetableDTO Get(Guid id);
        public TimetableDTO Check(TimetableDTO timetable);
        public void Update(TimetableDTO timetable);
        public void Delete(Guid id);
    }
}
