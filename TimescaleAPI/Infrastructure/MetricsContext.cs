using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Infrastructure;

public class MetricsContext(DbContextOptions<MetricsContext> options) : DbContext(options)
{
    public DbSet<Origin> Origins { get; set; }
    public DbSet<Value> Values { get; set; }
    public DbSet<Result> Results { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Origin>().HasKey(p => p.Id);
        modelBuilder.Entity<Value>().HasKey(p => p.Id);
        modelBuilder.Entity<Result>().HasKey(p => p.Id);
        
        modelBuilder.Entity<Origin>()
            .HasIndex(x => x.NameHash)
            .IsUnique();
    }
}