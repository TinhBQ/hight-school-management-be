using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
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

        public static IQueryable<Assignment> FilterSemesterAssigned(this IQueryable<Assignment> assignments, bool? isNotAssigned) =>
           isNotAssigned == null
           ? assignments
           : isNotAssigned == true ? assignments.Where(e => (e.TeacherId == null)) : assignments.Where(e => (e.TeacherId != null));

        public static IQueryable<Assignment> FilterClass(this IQueryable<Assignment> assignments, Guid? classId) =>
           classId == null
           ? assignments
           : assignments.Where(e => (e.ClassId == classId));

        public static IQueryable<Assignment> FilterTeacher(this IQueryable<Assignment> assignments, Guid? teacherId) =>
          teacherId == null
          ? assignments
          : assignments.Where(e => (e.TeacherId == teacherId));


        public static IQueryable<Assignment> FilterSubject(this IQueryable<Assignment> assignments, Guid? subjectId) =>
          subjectId == null
          ? assignments
          : assignments.Where(e => (e.SubjectId == subjectId));

        public static IQueryable<Assignment> JoinTable(this IQueryable<Assignment> assignments, bool isInclude) =>
           isInclude == false
           ? assignments
           : assignments
            .Include(a => a.Teacher)
            .Include(a => a.Subject)
            .Include(a => a.Class)
            .ThenInclude(a => a.HomeroomTeacher);


        public static IQueryable<Assignment> Search(this IQueryable<Assignment> assignments, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return assignments;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return assignments
                .Where(e => (e.Teacher.FirstName + " " + e.Teacher.MiddleName + " " + e.Teacher.LastName
                            + " " + e.Class.Name + " " + e.Subject.Name)
                            .ToLower().Contains(lowerCaseTerm)
                );
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
