using Lime.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lime.Infrastructure.Identity.Data.Context;

public class AuthDbContext : IdentityDbContext<LimeUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }
}