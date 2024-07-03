using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositorySubjectTeacherExtensions
    {
        public static IQueryable<SubjectTeacher> JoinTable(this IQueryable<SubjectTeacher> subjectTeachers, bool isInclude) =>
            isInclude == false
            ? subjectTeachers
            : subjectTeachers
                .Include(c => c.Subject)
                .Include(c => c.Teacher);

        public static IQueryable<SubjectTeacher> Search(this IQueryable<SubjectTeacher> subjectTeachers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return subjectTeachers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return subjectTeachers.Where(e => e.Subject.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<SubjectTeacher> Sort(this IQueryable<SubjectTeacher> subjectTeachers, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return subjectTeachers.OrderBy(e => e.Subject.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Teacher>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return subjectTeachers.OrderBy(e => e.Subject.Name);

            return subjectTeachers.OrderBy(orderQuery);
        }
    }
}
