using HerdFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HerdFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cow> Cows {get; set;}
}
