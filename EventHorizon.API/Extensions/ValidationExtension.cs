using EventHorizon.Application.Validation;
using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Contracts.Requests;
using FluentValidation;

namespace EventHorizon.API.Extensions
{
    public static class ValidationExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserRequest>, RegisterRequestValidator>();
            services.AddScoped<IValidator<AddEventRequest>, AddEventRequestValidator>();
            services.AddScoped<IValidator<UpdateEventRequest>, UpdateEventRequestValidator>();


            services.AddScoped<IValidator<AddCategoryRequest>, AddCategoryRequestValidator>();
            services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryRequestValidator>();

            return services;
        }
    }
}
