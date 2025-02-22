using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tick> Ticks { get; init; }
    public DbSet<User> Users { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SportRoute>().ToTable("SportRoutes");
        modelBuilder.Entity<TradRoute>().ToTable("TradRoutes");
        modelBuilder.Entity<IceClimb>().ToTable("IceClimbs");
        modelBuilder.Entity<AlpineRockRoute>().ToTable("AlpineRockRoutes");
        modelBuilder.Entity<Boulder>().ToTable("Boulders");
    }
}
