using Entities.DAOs;
using Entities.RequestFeatures;

namespace Services.Abstraction.IRepositoryServices
{
    public interface IAssignmentRepository
    {
        Task<PagedList<Assignment>> GetAllAssignmentWithPagedList(AssignmentParameters assignmentParameters, bool trackChanges);


        Task<IEnumerable<Assignment>> GetAllAssignment(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Assignment>> GetAllAssignmentBySubjectId(Guid subjectId, AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Class>> GetAssignmentWithClasses(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Teacher>> GetAssignmentWithTeahers(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Subject>> GetAssignmentWithSubjects(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Subject>> GetAssignmentWithSubjectsNotSameTeacher(AssignmentParameters assignmentParameters, bool trackChanges);

        Task<IEnumerable<Class>> GetAssignmentWithClassesBySubjectId(Guid subjectId, AssignmentParameters assignmentParameters, bool trackChanges);

        void CreateAssignment(Assignment assignment);

        void DeleteAssignment(Assignment assignment);
    }
}
