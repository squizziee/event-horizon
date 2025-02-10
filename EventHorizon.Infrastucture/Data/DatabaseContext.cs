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

        private IConfiguration _configuration;


        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfig());
        }
    }
}
