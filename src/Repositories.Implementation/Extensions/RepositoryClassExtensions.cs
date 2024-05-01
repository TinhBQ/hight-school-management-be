using Entities.DAOs;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositoryClassExtensions
    {
        /*public static IQueryable<Class> FilterEmployees(this IQueryable<Class> employees, uint minAge, uint maxAge) => 
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));*/

        public static IQueryable<Class> Search(this IQueryable<Class> classes, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return classes;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return classes.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Class> Sort(this IQueryable<Class> classes, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return classes.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Class>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return classes.OrderBy(e => e.Name);

            return classes.OrderBy(orderQuery);
        }
    }
}
