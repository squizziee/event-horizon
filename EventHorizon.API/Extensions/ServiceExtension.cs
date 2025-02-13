using EventHorizon.Infrastructure.Services;
using EventHorizon.Infrastructure.Services.Interfaces;

namespace EventHorizon.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IImageService, ImageService>();

            return services;
        }
    }
}
