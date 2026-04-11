using AridentIam.Application;
using AridentIam.Infrastructure;
using AridentIam.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddProblemDetails();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title       = "AridentIAM API",
            Version     = "v1",
            Description = "Identity and Access Management API"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token."
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                Array.Empty<string>()
            }
        });
    });

    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSettings["SecretKey"]
        ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();

    // ── CORS ─────────────────────────────────────────────────────────────────
    const string CorsPolicyName = "DefaultCorsPolicy";
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? [];

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(CorsPolicyName, policy =>
        {
            if (allowedOrigins.Length > 0)
                policy.WithOrigins(allowedOrigins);
            else
                policy.AllowAnyOrigin(); // development fallback

            policy.AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders("X-Correlation-Id");
        });
    });

    // ── Rate Limiting ─────────────────────────────────────────────────────────
    var rl = builder.Configuration.GetSection("RateLimiting");

    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("fixed", limiterOptions =>
        {
            limiterOptions.PermitLimit          = rl.GetValue("PermitLimit", 200);
            limiterOptions.Window               = TimeSpan.FromSeconds(rl.GetValue("WindowSeconds", 60));
            limiterOptions.QueueLimit           = rl.GetValue("QueueLimit", 10);
            limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        });

        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.OnRejected = async (context, token) =>
        {
            context.HttpContext.Response.StatusCode  = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/problem+json";
            await context.HttpContext.Response.WriteAsJsonAsync(new
            {
                title  = "Too Many Requests",
                status = 429,
                detail = "Rate limit exceeded. Please retry after a short delay."
            }, token);
        };
    });

    var app = builder.Build();

    // ── Exception handling (outermost) ────────────────────────────────────────
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // ── Security headers ──────────────────────────────────────────────────────
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"]        = "DENY";
        context.Response.Headers["X-XSS-Protection"]       = "1; mode=block";
        context.Response.Headers["Referrer-Policy"]        = "strict-origin-when-cross-origin";
        context.Response.Headers["Permissions-Policy"]     = "geolocation=(), microphone=()";

        if (!app.Environment.IsDevelopment())
            context.Response.Headers["Strict-Transport-Security"] =
                "max-age=31536000; includeSubDomains";

        await next();
    });

    app.UseHttpsRedirection();
    app.UseCors(CorsPolicyName);
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers().RequireRateLimiting("fixed");

    app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
    }).AllowAnonymous().RequireRateLimiting("fixed");

    Log.Information("AridentIAM API starting up");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "AridentIAM API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
