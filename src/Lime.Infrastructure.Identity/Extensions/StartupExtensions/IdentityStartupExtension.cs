using System.Text;

using Lime.Infrastructure.Identity.Configuration;
using Lime.Infrastructure.Identity.Data.Context;
using Lime.Infrastructure.Identity.Models;
using Lime.Persistence.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Lime.Infrastructure.Identity.Extensions.StartupExtensions;

public static class IdentityStartupExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IHostEnvironment env)
    {
        DatabaseOptions databaseOptions = services.BuildServiceProvider().GetRequiredService<DatabaseOptions>();
        JwtOptions jwtOptions = services.BuildServiceProvider().GetRequiredService<JwtOptions>();

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey));

        services.AddIdentity<LimeUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        services.Configure<JwtIssuerOptions>(options =>
        {
            options.Issuer = jwtOptions.Issuer;
            options.Audience = jwtOptions.Audience;
            options.SigningCredentials = signingCredentials;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(configureOptions =>
        {
            configureOptions.ClaimsIssuer = jwtOptions.Issuer;
            configureOptions.TokenValidationParameters = tokenValidationParameters;
            configureOptions.SaveToken = true;
        });

        services.AddAuthorization(options =>
        {
        });
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);

            if (!env.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IdentityStartupExtension).Assembly));
        return services;
    }

    public static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}