using Lime.Domain.Entities;
using Lime.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lime.Infrastructure.Identity.Data.Context;

public class AuthDbContext : IdentityDbContext<LimeIdentityUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
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