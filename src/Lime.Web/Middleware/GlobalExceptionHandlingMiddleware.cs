using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lime.Web.Middleware;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger logger;
    private readonly ApiBehaviorOptions options;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IOptions<ApiBehaviorOptions> options)
    {
        this.logger = logger;
        this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, e.Message);

            this.ApplyProblemDetailsDefaults(
                context,
                new ProblemDetails(),
                HttpStatusCode.InternalServerError);
        }
    }

    private async void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, HttpStatusCode statusCode)
    {
        problemDetails.Status ??= (int)statusCode;

        if (this.options.ClientErrorMapping.TryGetValue((int)statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        string json = JsonSerializer.Serialize(problemDetails);

        httpContext!.Response.StatusCode = (int)statusCode;
        httpContext!.Response.ContentType = "application/problem+json";
        await httpContext!.Response.WriteAsync(json);
    }
}