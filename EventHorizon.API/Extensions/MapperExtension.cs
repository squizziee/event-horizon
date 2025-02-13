using EventHorizon.Application.MapperProfiles;

namespace EventHorizon.API.Extensions
{
    public static class MapperExtension
    {
        public static IServiceCollection AddMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(UserMapperProfile),
                typeof(EventMapperProfile),
                typeof(CategoryMapperProfile),
                typeof(EventRequestToEntityMapperProfile),
                typeof(EventEntryMapperProfile),
                typeof(UserEventEntryMapperProfile)
            );

            return services;
        }
    }
}
