using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Entities;

namespace RouteTickrAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Tick> Ticks { get; init; }
    public DbSet<User> Users { get; init; }
    
    public DbSet<Climb> Climbs { get; init; }
    public DbSet<SportRoute> SportRoutes { get; init; }
    public DbSet<TradRoute> TradRoutes { get; init; }
    public DbSet<IceClimb> IceClimbs { get; init; }
    public DbSet<AlpineRockRoute> AlpineRockRoutes { get; init; }
    public DbSet<Boulder> Boulders { get; init; }
    
    public DbSet<LocationNode> Locations { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Climb>().ToTable("Climbs");
        modelBuilder.Entity<SportRoute>().ToTable("SportRoutes");
        modelBuilder.Entity<TradRoute>().ToTable("TradRoutes");
        modelBuilder.Entity<IceClimb>().ToTable("IceClimbs");
        modelBuilder.Entity<AlpineRockRoute>().ToTable("AlpineRockRoutes");
        modelBuilder.Entity<Boulder>().ToTable("Boulders");
        
        modelBuilder.Entity<Tick>()
            .HasOne(t => t.Climb)
            .WithMany()
            .HasForeignKey(t => t.ClimbId);

        modelBuilder.Entity<LocationNode>()
            .HasMany(l => l.Children)
            .WithOne(l => l.Parent)
            .HasForeignKey(l => l.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
}
