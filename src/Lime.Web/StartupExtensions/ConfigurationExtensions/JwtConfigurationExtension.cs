using Lime.Web.Configuration;

using Microsoft.Extensions.Options;

using Serilog;

namespace Lime.Web.StartupExtensions.ConfigurationExtensions;

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
                    throw ConfigurationValidator.GetStartupValidationException(ex);
                }
            });

        return services;
    }
}