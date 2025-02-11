using EventHorizon.Application.UseCases;
using EventHorizon.Application.UseCases.Interfaces;
using EventHorizon.Application.Validation;
using EventHorizon.Contracts.Requests;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Data.Repositories;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;
using EventHorizon.Infrastructure.Services;
using EventHorizon.Infrastructure.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventEntryRepository, EventEntryRepository>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IValidator<RegsiterUserRequest>, RegisterRequestValidator>();


builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
