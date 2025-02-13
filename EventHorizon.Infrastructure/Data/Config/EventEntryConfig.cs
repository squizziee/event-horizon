using EventHorizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHorizon.Infrastructure.Data.Config
{
    public class EventEntryConfig : IEntityTypeConfiguration<EventEntry>
    {
        public void Configure(EntityTypeBuilder<EventEntry> builder)
        {
            builder
               .HasKey(e => e.Id);

            builder
                .HasIndex(ee => new { ee.EventId, ee.UserId })
                .IsUnique();

            builder
                 .HasOne(ee => ee.Event)
                 .WithMany(e => e.Entries)
                 .HasForeignKey(ee => ee.EventId);
        }
    }
}
