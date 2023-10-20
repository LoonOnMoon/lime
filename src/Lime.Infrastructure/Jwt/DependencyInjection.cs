using Lime.Domain.Exceptions.Program;
using Lime.Infrastructure.Jwt.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lime.Infrastructure.Jwt;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services)
    {
        var options = services.AddOptions<JwtOptions>()
            .BindConfiguration(nameof(JwtOptions))
            .ValidateDataAnnotations()
            .Validate(
                o => o.ExpiresIn > TimeSpan.Zero,
                $"Validation failed for '{nameof(JwtOptions)}' members: '{nameof(JwtOptions.ExpiresIn)}' with the error: 'The field {nameof(JwtOptions.ExpiresIn)} must be greater than 00:00:00.'.")
            .ValidateOnStart();

        services.AddSingleton(sp =>
            {
                try
                {
                    return sp.GetRequiredService<IOptions<JwtOptions>>().Value;
                }
                catch (OptionsValidationException ex)
                {
                    throw new StartupException(ex);
                }
            });

        return services;
    }
}