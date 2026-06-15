using Serilog;
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

        app.UseHttpsRedirection();
        app.UseCors("Default");
        app.UseRateLimiter();

        if (app.Environment.IsDevelopment())
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
