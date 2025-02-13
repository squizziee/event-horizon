using EventHorizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHorizon.API.Extensions
{
    public static class DataExtension
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetValue<string>("NpgsqlConnectionString"));
                }
            );
            return services;
        }
    }
}
