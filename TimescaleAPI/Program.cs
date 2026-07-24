using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TimescaleAPI.API;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.ExceptionHandlers;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Services;
using TimescaleAPI.Application.Utilities;
using TimescaleAPI.Infrastructure;
using TimescaleAPI.Infrastructure.Repositories;

namespace TimescaleAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<MetricsContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Services.AddScoped<IResultRepository, ResultRepository>();
        builder.Services.AddScoped<IValueRepository, ValueRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        
        builder.Services.AddScoped<IValidator<TimescaleValueDto>, TimescaleValueValidator>();
        builder.Services.AddScoped<IResultCalculator, ResultCalculator>();
        builder.Services.AddScoped<IUploadService, UploadService>();
        builder.Services.AddScoped<IFilterService, FilterService>();
        builder.Services.AddScoped<IValueService, ValueService>();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
            });
        }
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MetricsContext>();
            db.Origins.Select(x => x.Id).Any(); 
            db.Database.CanConnect(); 
        }
        
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseRouting();
        
        app.RegisterTimescaleEndpoints();
        
        app.Run();
    }
}