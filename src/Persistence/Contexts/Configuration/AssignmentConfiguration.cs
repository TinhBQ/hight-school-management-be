using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.PeriodCount).HasDefaultValue(0);
            entity.Property(e => e.SchoolShift).HasConversion<int>();
            entity.Property(e => e.Semester).IsRequired();
            entity.Property(e => e.StartYear).IsRequired();
            entity.Property(e => e.EndYear).IsRequired();

            entity.HasOne(e => e.Teacher)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.TeacherId)
                .HasConstraintName("FK__Assignment__Teacher__TeacherId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);
            entity.HasOne(e => e.Class)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.ClassId)
                .HasConstraintName("FK__Assignment__Class__ClassId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
            entity.HasOne(e => e.Subject)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.SubjectId)
                .HasConstraintName("FK__Assignment__Subject__SubjectId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        }
    }
}
