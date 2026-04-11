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
        catch (Exception exception)
        {
            await HandleAsync(context, exception);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.ItemKey, out var value)
            ? value?.ToString()
            : null;

        var (statusCode, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
            ValidationException => (StatusCodes.Status422UnprocessableEntity, "Validation Failed"),
            DomainException => (StatusCodes.Status400BadRequest, "Domain Rule Violation"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
            OperationCanceledException => (StatusCodes.Status499ClientClosedRequest, "Request Cancelled"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        if (statusCode >= 500)
        {
            logger.LogError(
                exception,
                "Unhandled exception [{ExceptionType}] on {Method} {Path} with CorrelationId {CorrelationId}",
                exception.GetType().Name,
                context.Request.Method,
                context.Request.Path,
                correlationId);
        }
        else
        {
            logger.LogWarning(
                exception,
                "Handled exception [{ExceptionType}] on {Method} {Path} with CorrelationId {CorrelationId}",
                exception.GetType().Name,
                context.Request.Method,
                context.Request.Path,
                correlationId);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = environment.IsDevelopment()
                ? exception.ToString()
                : exception.Message,
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            problemDetails.Extensions["correlationId"] = correlationId;
        }

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray());
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails, JsonOptions));
    }
}