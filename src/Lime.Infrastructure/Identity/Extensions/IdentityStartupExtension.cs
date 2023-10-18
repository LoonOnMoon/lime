using System.Reflection;

using Lime.Application.Authentication;
using Lime.Infrastructure.Identity.Models;
using Lime.Infrastructure.Identity.Services;
using Lime.Infrastructure.Persistence.Configuration;

using Mapster;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lime.Infrastructure.Identity.Extensions;

public static class IdentityStartupExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IHostEnvironment env, TypeAdapterConfig typeAdapterConfig)
    {
        DatabaseOptions databaseOptions = services.BuildServiceProvider().GetRequiredService<DatabaseOptions>();

        // JwtOptions jwtOptions = services.BuildServiceProvider().GetRequiredService<JwtOptions>();

        // var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey));
        // var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());

        // services.AddSingleton<JwtTokenGenerator>();

        services.AddIdentity<LimeIdentityUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LimeIdentityDbContext>()
            .AddDefaultTokenProviders();

        // services.Configure<JwtIssuerOptions>(options =>
        // {
        //     options.Issuer = jwtOptions.Issuer;
        //     options.Audience = jwtOptions.Audience;
        //     options.SigningCredentials = signingCredentials;
        // });

        // var tokenValidationParameters = new TokenValidationParameters
        // {
        //     ValidateIssuer = true,
        //     ValidIssuer = jwtOptions.Issuer,
        //     ValidateAudience = true,
        //     ValidAudience = jwtOptions.Audience,
        //     ValidateIssuerSigningKey = true,
        //     IssuerSigningKey = signingKey,
        //     RequireExpirationTime = false,
        //     ValidateLifetime = true,
        //     ClockSkew = TimeSpan.Zero,
        // };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        // .AddJwtBearer(configureOptions =>
        // {
        //     configureOptions.ClaimsIssuer = jwtOptions.Issuer;
        //     configureOptions.TokenValidationParameters = tokenValidationParameters;
        //     configureOptions.SaveToken = true;
        // });

        services.AddAuthorization(options =>
        {
        });
        services.AddDbContext<LimeIdentityDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);

            if (!env.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
        services.AddScoped<IIdentityService, IdentityService>();
        return services;
    }

    public static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
    {
        app.UseAuthentication()
            .UseAuthorization();

        return app;
    }
}