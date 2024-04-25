using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasOne(a => a.Teacher)
                .WithMany(t => t.Assignments)
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
