namespace Lime.Web.Models.Exceptions;

public class StartupException : Exception
{
    public StartupException(string message)
    : base(message)
    {
    }
}