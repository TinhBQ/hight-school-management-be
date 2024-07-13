using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositoryClassExtensions
    {
        public static IQueryable<Class> FilterClasses(this IQueryable<Class> classes, uint? startYear, uint? endYear) =>
            startYear == null || endYear == null
            ? classes
            : classes.Where(e => (e.StartYear >= startYear && e.EndYear == endYear));

        public static IQueryable<Class> FilterClassesWithGrade(this IQueryable<Class> classes, uint? grade) =>
            grade == null
            ? classes
            : classes.Where(e => (e.Grade == grade));

        public static IQueryable<Class> FilterClassesWithIsAssignedHomeroom(this IQueryable<Class> classes, bool? isAssignedHomeroom) =>
            isAssignedHomeroom == null
            ? classes
            : isAssignedHomeroom == true
                ? classes.Where(e => (e.HomeroomTeacherId != null))
                : classes.Where(e => (e.HomeroomTeacherId == null));

        public static IQueryable<Class> JoinTable(this IQueryable<Class> classes, bool isInclude) =>
            isInclude == false
            ? classes
            : classes
            .Include(c => c.HomeroomTeacher)
            .Include(c => c.SubjectClasses)
                .ThenInclude(c => c.Subject)
            .Include(c => c.Assignments);

        public static IQueryable<Class> Search(this IQueryable<Class> classes, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return classes;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return classes.Where(e =>
                e.Name.ToLower().Contains(lowerCaseTerm) ||
                (e.HomeroomTeacher.FirstName + " " + e.HomeroomTeacher.MiddleName + " " + e.HomeroomTeacher.LastName).ToLower().Contains(lowerCaseTerm)
            );
        }

        public static IQueryable<Class> Sort(this IQueryable<Class> classes, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return classes.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Class>(orderByQueryString);

            Console.WriteLine(orderQuery);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return classes.OrderBy(e => e.Name);

            return classes.OrderBy(orderQuery);
        }
    }
}
