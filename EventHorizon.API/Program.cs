using EventHorizon.API.Extensions;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.MapperProfiles;
using EventHorizon.Application.UseCases;
using EventHorizon.Application.UseCases.EventCategories;
using EventHorizon.Application.UseCases.EventEntries;
using EventHorizon.Application.UseCases.Events;
using EventHorizon.Application.UseCases.Interfaces;
using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Application.Validation;
using EventHorizon.Contracts.Requests;
using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Data.Repositories;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;
using EventHorizon.Infrastructure.Helpers;
using EventHorizon.Infrastructure.Services;
using EventHorizon.Infrastructure.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(
	options =>
	{
		options.UseNpgsql(builder.Configuration.GetValue<string>("NpgsqlConnectionString"));
	}
);

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(
		options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = builder.Configuration["Jwt:AccessToken:Issuer"],
				ValidAudience = builder.Configuration["Jwt:AccessToken:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:AccessToken:Key"]!))
			};

			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
					if (!string.IsNullOrEmpty(accessToken))
						context.Token = accessToken;
					return Task.CompletedTask;
				}
			};
		}
	);


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
    .AddPolicy("ViewerPolicy", policy => policy.RequireRole("Viewer", "Admin"));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventEntryRepository, EventEntryRepository>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IImageService, ImageService>();


builder.Services.Configure<PaginationOptions>(builder.Configuration.GetSection("Application:Pagination"));
builder.Services.Configure<ImageUploadOptions>(builder.Configuration.GetSection("Infrastructure:ImageUpload"));


builder.Services.AddScoped<IValidator<RegisterUserRequest>, RegisterRequestValidator>();
builder.Services.AddScoped<IValidator<AddEventRequest>, AddEventRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateEventRequest>, UpdateEventRequestValidator>();


builder.Services.AddScoped<IValidator<AddCategoryRequest>, AddCategoryRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryRequestValidator>();


builder.Services.AddAutoMapper(
	typeof(UserMapperProfile),
	typeof(EventMapperProfile),
	typeof(CategoryMapperProfile),
	typeof(EventRequestToEntityMapperProfile),
	typeof(EventEntryMapperProfile)
);


builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();
builder.Services.AddScoped<IRefreshTokensUseCase, RefreshTokensUseCase>();
builder.Services.AddScoped<IGetUserDataUseCase, GetUserDataUseCase>();
builder.Services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();


builder.Services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
builder.Services.AddScoped<IGetEventUseCase, GetEventUseCase>();
builder.Services.AddScoped<ISearchEventsUseCase, SearchEventsUseCase>();
builder.Services.AddScoped<IAddEventUseCase, AddEventUseCase>();
builder.Services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
builder.Services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();


builder.Services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();
builder.Services.AddScoped<IGetCategoryUseCase, GetCategoryUseCase>();
builder.Services.AddScoped<IAddCategoryUseCase, AddCategoryUseCase>();
builder.Services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
builder.Services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();


builder.Services.AddScoped<IGetEventEntryUseCase, GetEventEntryUseCase>();
builder.Services.AddScoped<IGetEventEntriesUseCase, GetEventEntriesUseCase>();
builder.Services.AddScoped<IAddEventEntryUseCase, AddEventEntryUseCase>();
builder.Services.AddScoped<IDeleteEventEntryUseCase, DeleteEventEntryUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler();

app.UseStaticFiles();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
