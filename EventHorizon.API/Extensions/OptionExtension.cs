using EventHorizon.Application.Helpers;
using EventHorizon.Infrastructure.Helpers;

namespace EventHorizon.API.Extensions
{
    public static class OptionExtension
    {
        public static IServiceCollection AddConfigSectionOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationOptions>(configuration.GetSection("Application:Pagination"));
            services.Configure<ImageUploadOptions>(configuration.GetSection("Infrastructure:ImageUpload"));
            services.Configure<AdminCredentials>(configuration.GetSection("Application:AdminCredentials"));

            return services;
        }
    }
}
