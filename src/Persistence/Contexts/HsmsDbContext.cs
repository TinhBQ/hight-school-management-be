using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts.Configuration;

namespace Contexts
{
    public class HsmsDbContext(DbContextOptions<HsmsDbContext> options) : DbContext(options)
    {


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.ClassId);
                entity.Property(e => e.TeacherId);
                entity.Property(e => e.SubjectId);

                entity.Property(e => e.PeriodCount);

                entity.Property(e => e.SchoolShift)
                    .HasConversion<int>();
                entity.Property(e => e.StartYear);
                entity.Property(e => e.EndYear);
                entity.Property(e => e.Semester);
                entity.Property(e => e.IsDeleted);

                entity.Property(e => e.CreateAt);
                entity.Property(e => e.UpdateAt);

                entity.HasOne(d => d.Class).WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Teacher).WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Subject).WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(d => d.TimetableUnits).WithOne(p => p.Assignment)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });*/

            /*modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.HomeroomTeacherId);

                entity.Property(e => e.Name);
                entity.Property(e => e.Grade);
                entity.Property(e => e.SchoolShift)
                    .HasConversion<int>();
                entity.Property(e => e.StartYear);
                entity.Property(e => e.EndYear);
                entity.Property(e => e.PeriodCount);

                entity.Property(e => e.CreateAt);
                entity.Property(e => e.UpdateAt);

                entity.HasMany(d => d.SubjectClasses).WithOne(p => p.Class)
                    .HasForeignKey(d => d.ClassId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(d => d.Assignments).WithOne(p => p.Class)
                    .HasForeignKey(d => d.ClassId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.HomeroomTeacher).WithOne(p => p.Class)
                    .HasForeignKey<Teacher>(d => d.ClassId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });*/

            /*modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.Name);
                entity.Property(e => e.ShortName);

                entity.Property(e => e.CreateAt);
                entity.Property(e => e.UpdateAt);

                entity.HasMany(d => d.SubjectClasses).WithOne(p => p.Subject)
                    .HasForeignKey(d => d.SubjectId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(d => d.SubjectTeachers).WithOne(p => p.Subject)
                    .HasForeignKey(d => d.TeacherId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(d => d.Assignments).WithOne(p => p.Subject)
                    .HasForeignKey(d => d.SubjectId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });*/

            /* modelBuilder.Entity<SubjectClass>(entity =>
             {
                 entity.HasKey(e => e.Id);
                 entity.Property(e => e.Id);

                 entity.Property(e => e.SubjectId);
                 entity.Property(e => e.ClassId);
                 entity.Property(e => e.PeriodCount);

                 entity.HasOne(p => p.Subject).WithMany(p => p.SubjectClasses)
                     .HasForeignKey(d => d.SubjectId)
                     .OnDelete(DeleteBehavior.ClientSetNull);

                 entity.HasOne(p => p.Class).WithMany(p => p.SubjectClasses)
                     .HasForeignKey(d => d.ClassId)
                     .OnDelete(DeleteBehavior.ClientSetNull);

                 entity.Property(e => e.CreateAt);
                 entity.Property(e => e.UpdateAt);
             });*/

            /*modelBuilder.Entity<SubjectTeacher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.SubjectId);
                entity.Property(e => e.TeacherId);
                entity.Property(e => e.IsMain);

                entity.HasOne(p => p.Subject).WithMany(p => p.SubjectTeachers)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(p => p.Teacher).WithMany(p => p.SubjectTeachers)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.CreateAt);
                entity.Property(e => e.UpdateAt);
            });*/

            /*modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.ClassId);

                entity.Property(e => e.FirstName);
                entity.Property(e => e.MiddleName);
                entity.Property(e => e.LastName);
                entity.Property(e => e.ShortName);

                entity.Property(e => e.CreateAt);
                entity.Property(e => e.UpdateAt);

                entity.HasMany(d => d.SubjectTeachers).WithOne(p => p.Teacher)
                    .HasForeignKey(e => e.TeacherId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Class).WithOne(p => p.HomeroomTeacher)
                    .HasForeignKey<Class>(e => e.HomeroomTeacherId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(d => d.Assignments).WithOne(p => p.Teacher)
                    .HasForeignKey(d => d.TeacherId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });*/

            /*modelBuilder.Entity<TimetableUnit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);

                entity.Property(e => e.AssignmentId);
                entity.Property(e => e.TimetableId);

                entity.Property(e => e.SubjectName);
                entity.Property(e => e.ClassName);
                entity.Property(e => e.TeacherName);

                entity.Property(e => e.Priority);
                entity.Property(e => e.StartAt);

                entity.Property(e => e.CreateAt);
                entity.Property(e => e.UpdateAt);

                entity.HasOne(d => d.Timetable).WithMany(p => p.TimetableUnits)
                    .HasForeignKey(d => d.TimetableId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Assignment).WithMany(p => p.TimetableUnits)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });*/

            /* modelBuilder.Entity<Timetable>(entity =>
             {
                 entity.HasKey(e => e.Id);
                 entity.Property(e => e.Id);

                 entity.Property(e => e.Name);

                 entity.Property(e => e.CreateAt);
                 entity.Property(e => e.UpdateAt);

                 entity.HasMany(d => d.TimetableUnits).WithOne(p => p.Timetable)
                     .HasForeignKey(d => d.TimetableId)
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.ClientSetNull);
             });*/

            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectClassConfiguration());
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
