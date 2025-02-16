using EventHorizon.Application.UseCases.Dev;
using EventHorizon.Application.UseCases.EventCategories;
using EventHorizon.Application.UseCases.EventEntries;
using EventHorizon.Application.UseCases.Events;
using EventHorizon.Application.UseCases.Interfaces.Dev;
using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Application.UseCases.Users;

namespace EventHorizon.API.Extensions
{
    public static class UseCaseExtension
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRefreshTokensUseCase, RefreshTokensUseCase>();
            services.AddScoped<IGetUserDataUseCase, GetUserDataUseCase>();
            services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();


            services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
            services.AddScoped<IGetEventUseCase, GetEventUseCase>();
            services.AddScoped<ISearchEventsUseCase, SearchEventsUseCase>();
            services.AddScoped<IAddEventUseCase, AddEventUseCase>();
            services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
            services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();


            services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
            services.AddScoped<IGetCategoryUseCase, GetCategoryUseCase>();
            services.AddScoped<IAddCategoryUseCase, AddCategoryUseCase>();
            services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
            services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();


            services.AddScoped<IGetEventEntryUseCase, GetEventEntryUseCase>();
            services.AddScoped<IGetEventEntriesUseCase, GetEventEntriesUseCase>();
            services.AddScoped<IGetUserEntriesUseCase, GetUserEntriesUseCase>();
            services.AddScoped<IAddEventEntryUseCase, AddEventEntryUseCase>();
            services.AddScoped<IDeleteEventEntryUseCase, DeleteEventEntryUseCase>();

            services.AddScoped<ICreateAdminUseCase, CreateAdminUseCase>();
            services.AddScoped<ISeedDatabaseUseCase, SeedDatabaseUseCase>();

            return services;
        }
    }
}
