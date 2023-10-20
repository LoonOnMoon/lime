using System.Diagnostics;
using System.Text.Json;

using FluentValidation;

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
        catch (ValidationException ve)
        {
            Dictionary<string, string[]> errors =
                ve.Errors
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            var problemDetails = new ProblemDetails()
            {
                Title = "One or more validation errors occured.",
            };

            problemDetails.Extensions["errors"] = errors;

            this.ApplyProblemDetailsDefaults(
                context,
                problemDetails,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception e) when (e is IServiceException)
        {
            var se = (e as IServiceException)!;
            this.logger.LogDebug(e, e.Message);

            this.ApplyProblemDetailsDefaults(
                context,
                new ProblemDetails()
                {
                    Title = se.Title,
                },
                statusCode: (int)se.StatusCode);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, e.Message);

            this.ApplyProblemDetailsDefaults(
                context,
                new ProblemDetails(),
                statusCode: StatusCodes.Status500InternalServerError);
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

        string json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });

        httpContext!.Response.StatusCode = statusCode;
        httpContext!.Response.ContentType = "application/problem+json";
        await httpContext!.Response.WriteAsync(json);
    }
}