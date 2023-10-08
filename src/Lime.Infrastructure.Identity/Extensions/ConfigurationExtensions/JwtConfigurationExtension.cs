using Lime.Domain.Common.Exceptions.Program;
using Lime.Infrastructure.Identity.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lime.Infrastructure.Identity.Extensions.ConfigurationExtensions;

public static class JwtConfigurationExtension
{
    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>()
            .BindConfiguration(nameof(JwtOptions))
            .ValidateDataAnnotations();

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