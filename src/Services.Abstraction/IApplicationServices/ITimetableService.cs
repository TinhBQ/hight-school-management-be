using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITimetableService
    {
        public TimetableIndividual Generate(TimetableParameters parameters);
        public TimetableIndividual Get(Guid id, TimetableParameters parameters);
        public TimetableIndividual Check(TimetableIndividual timetable);
        public void Update(TimetableIndividual timetable);
        public void Delete(Guid id);
    }
}
