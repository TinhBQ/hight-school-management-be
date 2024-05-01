using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class TimetableUnitConfiguration : IEntityTypeConfiguration<TimetableUnit>
    {
        public void Configure(EntityTypeBuilder<TimetableUnit> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id);
            entity.Property(e => e.TeacherName).IsRequired();
            entity.Property(e => e.ClassName).IsRequired();
            entity.Property(e => e.SubjectName).IsRequired();
            entity.Property(e => e.Priority).HasConversion<int>().IsRequired();
            entity.Property(e => e.StartAt).IsRequired();

            entity.Property(e => e.TeacherId).IsRequired(false);
            entity.Property(e => e.ClassId).IsRequired(false);
            entity.Property(e => e.SubjectId).IsRequired(false);

            entity.HasOne(e => e.Timetable)
                .WithMany(e => e.TimetableUnits)
                .HasForeignKey(e => e.TimetableId)
                .HasConstraintName("FK__TimetableUnit__Timetable__TimetableId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
