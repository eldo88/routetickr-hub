using Microsoft.EntityFrameworkCore;
using RouteTickr.Entities;

namespace RouteTickr.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<Tick> Ticks { get; init; }
    public DbSet<User> Users { get; init; }
    
    public DbSet<Climb> Climbs { get; init; }
    public DbSet<SportRoute> SportRoutes { get; init; }
    public DbSet<TradRoute> TradRoutes { get; init; }
    public DbSet<IceClimb> IceClimbs { get; init; }
    public DbSet<AlpineRockRoute> AlpineRockRoutes { get; init; }
    public DbSet<Boulder> Boulders { get; init; }
    
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
        
        
        base.OnModelCreating(modelBuilder);
    }
}