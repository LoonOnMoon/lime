using Lime.Infrastructure.Identity.Data;
using Lime.Infrastructure.Identity.Models;

using Microsoft.EntityFrameworkCore;

namespace Lime.Web.StartupExtensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                // if (!env.IsProduction())
                // {
                //     options.EnableDetailedErrors();
                //     options.EnableSensitiveDataLogging();
                // }
            });

            // services.AddDbContext<ApplicationDbContext>(options =>
            // {
            //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                // if (!env.IsProduction())
                // {
                //     options.EnableDetailedErrors();
                //     options.EnableSensitiveDataLogging();
                // }
            // });
            return services;
        }
    }
}