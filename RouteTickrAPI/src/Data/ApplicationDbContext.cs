using Microsoft.EntityFrameworkCore;
using RouteTickrAPI.Models;

namespace RouteTickrAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tick> Ticks { get; init; }
    public DbSet<User> Users { get; init; }
}
