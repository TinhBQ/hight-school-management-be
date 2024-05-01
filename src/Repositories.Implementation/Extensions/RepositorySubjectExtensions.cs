using Entities.DAOs;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositorySubjectExtensions
    {
        /*public static IQueryable<Class> FilterEmployees(this IQueryable<Class> employees, uint minAge, uint maxAge) => 
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));*/

        public static IQueryable<Subject> Search(this IQueryable<Subject> subjects, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return subjects;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return subjects.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Subject> Sort(this IQueryable<Subject> subjects, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return subjects.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Subject>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return subjects.OrderBy(e => e.Name);

            return subjects.OrderBy(orderQuery);
        }
    }
}
