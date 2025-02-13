using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventHorizon.Infrastructure.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventEntry> EventEntries { get; set; }
        public DbSet<Event> Events { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new EventConfig());
            modelBuilder.ApplyConfiguration(new EventEntryConfig());
            modelBuilder.ApplyConfiguration(new EventCategoryConfig());
        }
    }
}
