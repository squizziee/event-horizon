using EventHorizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHorizon.Infrastructure.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder
                .Property(u => u.FirstName)
                .HasMaxLength(50);

            builder
                .Property(u => u.LastName)
                .HasMaxLength(100);

            builder
                .HasMany(u => u.Entries)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        }
    }
}
