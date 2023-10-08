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
}