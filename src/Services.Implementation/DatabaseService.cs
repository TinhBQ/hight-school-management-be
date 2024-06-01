using Contexts;
using CsvHelper;
using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction.IApplicationServices;
using System.Globalization;

namespace Services.Implementation
{
    public class DatabaseService(HsmsDbContext context) : IDatabaseService
    {
        private readonly HsmsDbContext _context = context;
        private class AssignmentCSV()
        {
            public string Teacher { get; set; } = null!;
            public string Subject { get; set; } = null!;
            public string Class { get; set; } = null!;
        };

        public void CreateAssignments()
        {
            var path = "D:\\Workspace\\dotnet-asp\\10-be\\Assignment.csv";
            var reader = new StreamReader(path);
            var teachers = _context.Teachers.Include(t => t.SubjectTeachers).AsNoTracking().ToList();
            var classes = _context.Classes.Include(c => c.SubjectClasses).AsNoTracking().ToList();
            var subjects = _context.Subjects.AsNoTracking().ToList();
            var assignments = new List<Assignment>();
            var records = new List<AssignmentCSV>();

            using (reader)
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                while (csv.Read())
                    records.Add(csv.GetRecord<AssignmentCSV>());
            }

            foreach (var record in records)
            {
                foreach (var @className in record.Class.Split(","))
                {
                    var id = Guid.NewGuid();
                    var teacherId = teachers.First(t => t.ShortName == record.Teacher).Id;
                    var subjectId = subjects.First(s => s.ShortName == record.Subject).Id;
                    var classId = classes.First(c => c.Name == @className).Id;
                    var periodCount = classes
                        .First(c => c.Name == @className)
                        .SubjectClasses
                        .First(sc => sc.SubjectId == subjects.First(s => s.ShortName == record.Subject).Id).PeriodCount;
                    var schoolShift = classes.First(c => c.Name == @className).SchoolShift;
                    assignments.Add(new Assignment()
                    {
                        Id = id,
                        TeacherId = teacherId,
                        SubjectId = subjectId,
                        ClassId = classId,
                        SchoolShift = schoolShift,
                        Semester = 1,
                        StartYear = 2023,
                        EndYear = 2024,
                        PeriodCount = periodCount
                    });
                }
            }
            _context.AddRange(assignments);
            _context.SaveChanges();
        }

        public void DeleteAssignments()
        {
            var assignments = _context.Assignments.ToList();
            _context.RemoveRange(assignments);
            _context.SaveChanges();
        }

        public void UpdateHomeroomTeacher()
        {
            var classes = _context.Classes.Include(c => c.HomeroomTeacher).ToList();
            foreach (var @class in classes)
                @class.HomeroomTeacher.ClassId = @class.Id;
            _context.SaveChanges();
        }
    }
}
