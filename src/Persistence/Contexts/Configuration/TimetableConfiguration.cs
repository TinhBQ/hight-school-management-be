using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
    {
        public void Configure(EntityTypeBuilder<Timetable> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.StartYear).IsRequired();
            entity.Property(e => e.EndYear).IsRequired();
            entity.Property(e => e.Semester).IsRequired();
            entity.Property(e => e.Parameters);
        }
    }
}
