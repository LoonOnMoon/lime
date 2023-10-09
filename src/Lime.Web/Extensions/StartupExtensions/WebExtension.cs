using Lime.Web.Common.Mapping;
using Lime.Web.Middleware;

using Microsoft.AspNetCore.Mvc.Versioning;

namespace Lime.Web.Extensions.StartupExtensions;

public static class WebExtension
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>()
            .AddMappings()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddApiVersioning(opt =>
                {
                    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                    opt.ReportApiVersions = true;
                    opt.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("x-api-version"),
                        new MediaTypeApiVersionReader("x-api-version"));
                })
            .AddControllers();

        return services;
    }
}