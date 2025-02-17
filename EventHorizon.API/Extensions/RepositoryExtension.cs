using EventHorizon.Domain.Interfaces.Repositories;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Data.Repositories;

namespace EventHorizon.API.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventEntryRepository, EventEntryRepository>();
            services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
