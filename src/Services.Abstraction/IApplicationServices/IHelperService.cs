using Entities.DAOs;
using Entities.DTOs.CRUD;
using Entities.RequestFeatures;

namespace Services.Abstraction.IApplicationServices
{
    public interface IHelperService
    {
        Task<Class> GetClassAndCheckIfItExists(Guid? id, bool trackChanges);
        Task<Teacher> GetTeacherAndCheckIfItExists(Guid? id, bool trackChanges);
        Task<Subject> GetSubjectAndCheckIfItExists(Guid? id, bool trackChanges);
        Task<SubjectClass> GetSubjectClassAndCheckIfItExists(Guid? id, bool trackChanges);
        Task<SubjectTeacher> GetSubjectTeacherAndCheckIfItExists(Guid? id, bool trackChanges);

        Task<Assignment> GetAssignmentAndCheckIfItExists(Guid? id, bool trackChanges);

        Task<Assignment?> GetAssignmentAndCheckIfItExists(Guid? classId, Guid? subjectId, bool trackChanges);
    }
}
