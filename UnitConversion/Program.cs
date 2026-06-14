using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using UnitConversion.Middleware;
using UnitConversion.Swagger;
using UnitConversion.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers()
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        SwaggerDescriptions.ApiVersion,
        new OpenApiInfo
        {
            Title = SwaggerDescriptions.ApiTitle,
            Version = SwaggerDescriptions.ApiVersion,
            Description = SwaggerDescriptions.BuildApiDescription()
        });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    options.SchemaFilter<ConversionSchemaFilter>();
    options.OperationFilter<ConversionExamplesOperationFilter>();
});

builder.Services.AddScoped<ConversionRequestValidator>();
builder.Services.AddScoped<IConversionService, ConversionService>();

builder.Services.AddScoped<IUnitConverter, LengthConverter>();
builder.Services.AddScoped<IUnitConverter, TemperatureConverter>();
builder.Services.AddScoped<IUnitConverter, WeightConverter>();
builder.Services.AddScoped<IUnitConverter, VolumeConverter>();
builder.Services.AddScoped<IUnitConverter, AreaConverter>();
builder.Services.AddScoped<IUnitConverter, SpeedConverter>();
builder.Services.AddScoped<IUnitConverter, TimeConverter>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        $"/swagger/{SwaggerDescriptions.ApiVersion}/swagger.json",
        SwaggerDescriptions.ApiTitle);
    options.DocumentTitle = SwaggerDescriptions.ApiTitle;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
