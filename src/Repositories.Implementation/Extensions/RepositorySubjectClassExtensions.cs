using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositorySubjectClassExtensions
    {
        /*public static IQueryable<Class> FilterEmployees(this IQueryable<Class> employees, uint minAge, uint maxAge) => 
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));*/

        public static IQueryable<SubjectClass> JoinTable(this IQueryable<SubjectClass> subjectClasses, bool isInClude)
        {
            return isInClude
                ? subjectClasses.Include(s => s.Class).Include(s => s.Subject)
                : subjectClasses;
        }

        public static IQueryable<SubjectClass> Search(this IQueryable<SubjectClass> subjectClasses, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return subjectClasses;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return subjectClasses
                .Where(e => (e.Class.Name + " " + e.Subject.Name + " " + e.Subject.ShortName)
                .ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<SubjectClass> Sort(this IQueryable<SubjectClass> subjectClasses, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return subjectClasses.OrderBy(e => e.Class.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Teacher>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return subjectClasses.OrderBy(e => e.Class.Name);

            return subjectClasses.OrderBy(orderQuery);
        }
    }
}
