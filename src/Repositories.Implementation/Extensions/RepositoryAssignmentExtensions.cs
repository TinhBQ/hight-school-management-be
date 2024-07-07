using Entities.DAOs;
using System.Linq.Dynamic.Core;

namespace Repositories.Implementation.Extensions
{
    public static class RepositoryAssignmentExtensions
    {
        public static IQueryable<Assignment> FilterYears(this IQueryable<Assignment> assignments, uint? startYear, uint? endYear) =>
            startYear == null || endYear == null
            ? assignments
            : assignments.Where(e => (e.StartYear >= startYear && e.EndYear == endYear));


        public static IQueryable<Assignment> FilterSemester(this IQueryable<Assignment> assignments, uint? semester) =>
           semester == null
           ? assignments
           : assignments.Where(e => (e.Semester == semester));

        public static IQueryable<Assignment> FilterClass(this IQueryable<Assignment> assignments, Guid? classId) =>
           classId == null
           ? assignments
           : assignments.Where(e => (e.ClassId == classId));


        

        public static IQueryable<Assignment> Search(this IQueryable<Assignment> assignments, string searchTerm)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrWhiteSpace(searchTerm))
            //    return assignments;

            //var lowerCaseTerm = searchTerm.Trim().ToLower();

            //return assignments;
        }

        public static IQueryable<Assignment> Sort(this IQueryable<Assignment> assignments, string orderByQueryString)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrWhiteSpace(orderByQueryString))
            //    return assignments;

            //var orderQuery = OrderQueryBuilder.CreateOrderQuery<Assignment>(orderByQueryString);

            //if (string.IsNullOrWhiteSpace(orderQuery))
            //    return assignments;

            //return assignments.OrderBy(orderQuery);
        }
    }
}
