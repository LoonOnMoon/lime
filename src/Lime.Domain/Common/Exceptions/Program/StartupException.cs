using Microsoft.Extensions.Options;

namespace Lime.Domain.Common.Exceptions.Program;

public class StartupException : Exception
{
    public StartupException(string message)
    : base(message)
    {
    }

    public StartupException(OptionsValidationException ex)
    : this(GetMessage(ex))
    {
    }

    private static string GetMessage(OptionsValidationException ex)
    {
        string message = $"{Environment.NewLine}\t{string.Join($"{Environment.NewLine}\t", ex.Failures)}{Environment.NewLine}";

        return message;
    }
}