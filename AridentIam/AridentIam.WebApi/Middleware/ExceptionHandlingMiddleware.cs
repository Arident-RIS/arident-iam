using AridentIam.Application.Common.Exceptions;
using AridentIam.Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AridentIam.WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundException          => (StatusCodes.Status404NotFound,            "Resource Not Found"),
            ConflictException          => (StatusCodes.Status409Conflict,            "Conflict"),
            ValidationException        => (StatusCodes.Status422UnprocessableEntity, "Validation Failed"),
            DomainException            => (StatusCodes.Status400BadRequest,          "Domain Rule Violation"),
            UnauthorizedAccessException=> (StatusCodes.Status403Forbidden,           "Forbidden"),
            OperationCanceledException => (StatusCodes.Status499ClientClosedRequest, "Request Cancelled"),
            _                          => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        if (statusCode >= 500)
        {
            logger.LogError(exception,
                "Unhandled exception [{ExceptionType}] on {Method} {Path}",
                exception.GetType().Name,
                context.Request.Method,
                context.Request.Path);
        }
        else
        {
            logger.LogWarning(exception,
                "Handled exception [{ExceptionType}] on {Method} {Path}",
                exception.GetType().Name,
                context.Request.Method,
                context.Request.Path);
        }

        var problemDetails = new ProblemDetails
        {
            Status   = statusCode,
            Title    = title,
            Detail   = environment.IsDevelopment() ? exception.ToString() : exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());
        }

        context.Response.StatusCode  = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, JsonOptions));
    }
}
