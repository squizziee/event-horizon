using EventHorizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHorizon.Infrastructure.Data.Config
{
    public class EventCategoryConfig : IEntityTypeConfiguration<EventCategory>
    {
        public void Configure(EntityTypeBuilder<EventCategory> builder)
        {
            builder
               .HasKey(e => e.Id);

            builder
                 .Property(e => e.Name)
                 .HasMaxLength(150);

            builder
                 .Property(e => e.Description)
                 .HasMaxLength(1000);
        }
    }
}
