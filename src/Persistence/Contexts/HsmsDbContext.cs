using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts.Configuration;

namespace Contexts
{
    public class HsmsDbContext(DbContextOptions<HsmsDbContext> options) : DbContext(options)
    {


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectClassConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectTeacherConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new TimetableConfiguration());
            modelBuilder.ApplyConfiguration(new TimetableUnitConfiguration());
        }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<SubjectClass> SubjectClasses { get; set; }

        public DbSet<SubjectTeacher> SubjectTeachers { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Timetable> Timetables { get; set; }

        public DbSet<TimetableUnit> TimetablesUnits { get; set; }
    }
}
