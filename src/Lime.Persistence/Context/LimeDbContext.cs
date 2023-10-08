using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lime.Persistence.Context;

public class LimeDbContext : DbContext
{
    public LimeDbContext(DbContextOptions<LimeDbContext> options)
        : base(options)
    {
    }
}