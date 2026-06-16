using Serilog;
using UnitConversion.Configuration;
using UnitConversion.Swagger;

namespace UnitConversion.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseExceptionHandler();

        if (!app.Environment.IsDevelopment())
            app.UseHsts();

        app.UseSecurityHeaders();

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
            };
        });

        if (IsHttpsRedirectionEnabled(app))
            app.UseHttpsRedirection();

        app.UseCors("Default");
        app.UseRateLimiter();

        if (IsSwaggerEnabled(app))
            EnableSwaggerUi(app);

        app.UseAuthorization();

        app.MapControllers()
            .RequireRateLimiting(ServiceCollectionExtensions.RateLimitPolicyName);

        app.MapHealthChecks("/health").DisableRateLimiting();
        app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        }).DisableRateLimiting();

        app.MapPrometheusScrapingEndpoint().DisableRateLimiting();

        return app;
    }

    private static bool IsSwaggerEnabled(WebApplication app) =>
        app.Environment.IsDevelopment() ||
        app.Configuration.GetValue<bool>($"{SwaggerOptions.SectionName}:Enabled");

    private static void EnableSwaggerUi(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
                $"/swagger/{SwaggerDescriptions.ApiVersion}/swagger.json",
                SwaggerDescriptions.ApiTitle);
            options.DocumentTitle = SwaggerDescriptions.ApiTitle;
        });
    }

    private static bool IsHttpsRedirectionEnabled(WebApplication app)
    {
        var configuredUrls = app.Configuration["urls"]
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
            ?? string.Empty;

        if (configuredUrls.Contains("https://", StringComparison.OrdinalIgnoreCase))
            return true;

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT")))
            return true;

        return app.Configuration.GetValue<int?>("HttpsRedirection:HttpsPort") is > 0;
    }

    private static void UseSecurityHeaders(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Referrer-Policy", "no-referrer");
            await next();
        });
    }
}
