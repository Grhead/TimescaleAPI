using System.Text;
using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Infrastructure;

public class MetricsContext : DbContext
{
    public DbSet<Origin> Origins { get; set; }
    public DbSet<Value> Values { get; set; }
    public DbSet<Result> Results { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Origin>().HasKey(p => p.Id);
        modelBuilder.Entity<Value>().HasKey(p => p.Id);
        modelBuilder.Entity<Result>().HasKey(p => p.Id);
    }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var host = Environment.GetEnvironmentVariable("pg_host");
        var port = Environment.GetEnvironmentVariable("pg_port");
        var database = Environment.GetEnvironmentVariable("pg_database");
        var user = Environment.GetEnvironmentVariable("pg_user");
        var password = Environment.GetEnvironmentVariable("pg_password");
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Host={host};Port={port};Database={database};Username={user};Password={password};");
        optionsBuilder.UseNpgsql(stringBuilder.ToString());
    }
}