using EventHorizon.API.Extensions;
using EventHorizon.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowClientOrigin",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
{
    return null;
};

builder.Services.AddDatabaseContext(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddPolicyAuthorization();

builder.Services.AddGlobalExceptionHandling();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddConfigSectionOptions(builder.Configuration);

builder.Services.AddValidators();

builder.Services.AddMapperProfiles();

builder.Services.AddUseCases();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	// apply migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler();

app.UseStaticFiles();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseCors("AllowClientOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
