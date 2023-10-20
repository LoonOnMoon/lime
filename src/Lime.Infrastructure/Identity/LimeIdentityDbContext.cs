using Lime.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lime.Infrastructure.Identity;

public class LimeIdentityDbContext : IdentityDbContext<LimeIdentityUser>
{
    public LimeIdentityDbContext(DbContextOptions<LimeIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<LimeIdentityUser>(
        //     ob =>
        //     {
        //         ob.HasOne(o => o.User).WithOne()
        //             .HasForeignKey<User>(o => o.Id);
        //     });
        base.OnModelCreating(modelBuilder);
    }
}