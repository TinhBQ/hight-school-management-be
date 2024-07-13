using Entities.DAOs;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositoryTeacherExtensions
    {
        public static IQueryable<Teacher> Search(this IQueryable<Teacher> teachers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return teachers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return teachers
                .Where(e => (e.FirstName + " " + e.MiddleName + " " + e.LastName + " " + e.ShortName)
                .ToLower().Contains(lowerCaseTerm));
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

        public static IQueryable<Teacher> FilterTeachersWithIsAssignedHomeroom(this IQueryable<Teacher> teachers, bool? isAssignedHomeroom, uint? startYear, uint? endYear) =>
            isAssignedHomeroom == null || startYear == null || endYear == null
            ? teachers
            : isAssignedHomeroom == true
                ? teachers.Where(e => (e.ClassId != null && e.Classes.Any(x => x.StartYear == startYear) && e.Classes.Any(x => x.EndYear == endYear)))
                : teachers.Where(e => (e.ClassId == null || (!e.Classes.Any(x => x.StartYear == startYear) && !e.Classes.Any(x => x.EndYear == endYear))));
    }
}
