namespace Lime.Web.Configuration;

public sealed class DatabaseOptions
{
    public const string ConnectionStringName = "Lime";

    public required string ConnectionString { get; set; }
}