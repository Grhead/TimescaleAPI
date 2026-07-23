using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TimescaleAPI.API;
using TimescaleAPI.Application;
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
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        
        builder.Services.AddScoped<IValidator<TimescaleData>, TimescaleDataValidator>();
        builder.Services.AddScoped<IResultCalculator, ResultCalculator>();
        builder.Services.AddScoped<UploadService>();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.RegisterTimescaleEndpoints();
        app.UseExceptionHandler();
        
        app.Run();
    }
}