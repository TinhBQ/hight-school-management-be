using Entities.DAOs;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositoryTeacherExtensions
    {
        /*public static IQueryable<Class> FilterEmployees(this IQueryable<Class> employees, uint minAge, uint maxAge) => 
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));*/

        public static IQueryable<Teacher> Search(this IQueryable<Teacher> teachers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return teachers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return teachers.Where(e => e.FirstName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Teacher> Sort(this IQueryable<Teacher> teachers, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return teachers.OrderBy(e => e.FirstName);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Teacher>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return teachers.OrderBy(e => e.FirstName);

            return teachers.OrderBy(orderQuery);
        }
    }
}
