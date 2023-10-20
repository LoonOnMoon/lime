using Lime.Domain.User;

using Microsoft.EntityFrameworkCore;

namespace Lime.Infrastructure.Persistence;

public class LimeDbContext : DbContext
{
    public LimeDbContext(DbContextOptions<LimeDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}