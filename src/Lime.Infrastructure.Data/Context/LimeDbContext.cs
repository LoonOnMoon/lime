using Microsoft.EntityFrameworkCore;

namespace Lime.Infrastructure.Data.Context;

public class LimeDbContext : DbContext
{
    public LimeDbContext(DbContextOptions<LimeDbContext> options)
        : base(options)
    {
    }
}