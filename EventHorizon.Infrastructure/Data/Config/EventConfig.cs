using EventHorizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHorizon.Infrastructure.Data.Config
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                 .Property(e => e.Name)
                 .HasMaxLength(150);

            builder
                 .Property(e => e.Description)
                 .HasMaxLength(1000);

            builder
                 .Property(e => e.Address)
                 .HasMaxLength(250);

            builder
                 .HasOne(e => e.Category)
                 .WithMany(c => c.Events)
                 .HasForeignKey(e => e.CategoryId);
        }
    }
}
