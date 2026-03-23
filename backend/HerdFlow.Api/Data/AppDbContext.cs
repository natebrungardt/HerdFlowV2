using Microsoft.EntityFrameworkCore;
using HerdFlow.Api.Models;

namespace HerdFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cow> Cows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cow>()
            .Property(c => c.HealthStatus)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}
