using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface ITimetableService
    {
        public TimetableIndividual Generate(TimetableParameters parameters);
        public Task<TimetableIndividual> Get(Guid id, TimetableParameters parameters);
        public Task<TimetableIndividual> Check(Guid timetableId, TimetableParameters parameters);
        public Task Update(TimetableIndividual timetable);
        public Task Delete(Guid id);
    }
}
