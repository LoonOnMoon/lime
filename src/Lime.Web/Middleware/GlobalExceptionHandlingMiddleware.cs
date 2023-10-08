using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

using Lime.Application.Common.Errors;

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
            var (statusCode, message) = e switch
            {
                IServiceException se => ((int)se.StatusCode, se.Title),
                _ => (StatusCodes.Status500InternalServerError, null),
            };

            if (e is not IServiceException)
            {
                this.logger.LogError(e, e.Message);
            }

            this.ApplyProblemDetailsDefaults(
                context,
                new ProblemDetails()
                {
                    Title = message,
                },
                statusCode: statusCode);
        }
    }

    private async void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (this.options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
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

        httpContext!.Response.StatusCode = statusCode;
        httpContext!.Response.ContentType = "application/problem+json";
        await httpContext!.Response.WriteAsync(json);
    }
}