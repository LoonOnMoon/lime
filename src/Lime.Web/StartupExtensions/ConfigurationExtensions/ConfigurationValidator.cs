using Lime.Web.Models.Exceptions;

using Microsoft.Extensions.Options;

namespace Lime.Web.StartupExtensions.ConfigurationExtensions;

public static class ConfigurationValidator
{
    public static StartupException GetStartupValidationException(OptionsValidationException ex)
    {
        string message = GetValidationMessage(ex);

        return new StartupException(message);
    }

    // TODO: Move this to ConfigurationValidationStartupException
    private static string GetValidationMessage(OptionsValidationException ex)
    {
        string message = $"{Environment.NewLine}\t{string.Join($"{Environment.NewLine}\t", ex.Failures)}{Environment.NewLine}";

        return message;
    }
}