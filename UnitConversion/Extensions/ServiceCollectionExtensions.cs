using System.Reflection;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using UnitConversion.Configuration;
using UnitConversion.Converters;
using UnitConversion.Middleware;
using UnitConversion.Services;
using UnitConversion.Services.Interfaces;
using UnitConversion.Swagger;
using UnitConversion.Validators;

namespace UnitConversion.Extensions;

public static class ServiceCollectionExtensions
{
    public const string RateLimitPolicyName = "fixed";

    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddOptions<CorsOptions>()
            .Bind(configuration.GetSection(CorsOptions.SectionName));

        services.AddOptions<RateLimitingOptions>()
            .Bind(configuration.GetSection(RateLimitingOptions.SectionName));

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Detail = "One or more validation errors occurred."
                    };

                    problemDetails.Extensions["errorCode"] = "VALIDATION_ERROR";

                    return new BadRequestObjectResult(problemDetails);
                };
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            options.SchemaFilter<ConversionSchemaFilter>();
            options.OperationFilter<ConversionExamplesOperationFilter>();
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        services.AddHealthChecks()
            .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: ["ready"]);

        services.AddCors(options =>
        {
            options.AddPolicy("Default", policy =>
            {
                var corsOptions = configuration
                    .GetSection(CorsOptions.SectionName)
                    .Get<CorsOptions>() ?? new CorsOptions();

                if (corsOptions.AllowedOrigins.Length > 0)
                {
                    policy.WithOrigins(corsOptions.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        var rateLimitOptions = configuration
            .GetSection(RateLimitingOptions.SectionName)
            .Get<RateLimitingOptions>() ?? new RateLimitingOptions();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status429TooManyRequests,
                        Title = "Too many requests",
                        Detail = "Rate limit exceeded. Please try again later.",
                        Type = "https://tools.ietf.org/html/rfc6585#section-4",
                        Extensions = { ["errorCode"] = "RATE_LIMIT_EXCEEDED" }
                    },
                    cancellationToken);
            };

            options.AddPolicy(RateLimitPolicyName, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitOptions.PermitLimit,
                        Window = TimeSpan.FromSeconds(rateLimitOptions.WindowSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
        });

        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddPrometheusExporter();
            });

        services.AddScoped<ConversionRequestValidator>();
        services.AddScoped<IConversionService, ConversionService>();

        services.AddScoped<IUnitConverter, LengthConverter>();
        services.AddScoped<IUnitConverter, TemperatureConverter>();
        services.AddScoped<IUnitConverter, WeightConverter>();
        services.AddScoped<IUnitConverter, VolumeConverter>();
        services.AddScoped<IUnitConverter, AreaConverter>();
        services.AddScoped<IUnitConverter, SpeedConverter>();
        services.AddScoped<IUnitConverter, TimeConverter>();

        return services;
    }
}
