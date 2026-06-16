# Unit Conversion API

ASP.NET Core Web API for converting numeric values between units of measurement across length, weight, temperature, volume, area, speed, and time.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Optional: [Docker](https://www.docker.com/) for containerized runs

## Run locally

From the repository root:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project UnitConversion
```

When started with `dotnet run`, the app loads `UnitConversion/Properties/launchSettings.json` and uses the default **`http`** launch profile:

| Setting | Value |
|---------|-------|
| URL | `http://localhost:5053` |
| Environment | `Development` |
| Swagger UI | `http://localhost:5053/swagger` |

Leave the terminal open while testing. Press **Ctrl+C** to stop the server.

### Visual Studio

1. Set **UnitConversion** as the startup project.
2. Select the **`http`** or **`https`** launch profile in the toolbar (not **Run Program.cs**).
3. Press **F5**. Swagger opens automatically when using a profile with `launchUrl: swagger`.

The **`https`** profile listens on `https://localhost:7113` and `http://localhost:5053`.

### Do not run `Program.cs` or the `.exe` directly

Running `Program.cs` or `bin/Debug/net9.0/UnitConversion.exe` directly skips launch settings. That starts the app in **Production** on **port 5000**, disables Swagger, and is not the intended local workflow.

### Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| `address already in use` on port 5053 | Another instance is still running | Stop it with **Ctrl+C** in the original terminal, or run `Get-Process UnitConversion \| Stop-Process -Force` in PowerShell |
| `ERR_CONNECTION_REFUSED` in the browser | The API is not running | Start it with `dotnet run --project UnitConversion` and wait for `Now listening on: http://localhost:5053` |
| Swagger returns 404 | App is running in Production (port 5000) | Restart with `dotnet run --project UnitConversion` |

### Other endpoints

- Health check: `GET /health`
- Readiness check: `GET /health/ready`
- Prometheus metrics: `GET /metrics`

## Swagger (interactive API documentation)

Swagger UI is available in the **Development** environment only. It is powered by [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) and documents API version **v1**.

### Open Swagger UI

1. Start the API with `dotnet run --project UnitConversion`.
2. Confirm the console shows `Hosting environment: Development`.
3. Open **http://localhost:5053/swagger** in a browser.

### Try the API from Swagger

1. Expand **GET /api/v1/Categories** → **Try it out** → **Execute** to list supported categories and units.
2. Expand **POST /api/v1/Conversion** → **Try it out**.
3. Edit the request body, for example:

   ```json
   {
     "category": "length",
     "fromUnit": "kilometers",
     "toUnit": "miles",
     "value": 1
   }
   ```

4. Click **Execute** and inspect the response body and status code.

### Swagger implementation

| Component | Location | Purpose |
|-----------|----------|---------|
| Pipeline registration | `Extensions/WebApplicationExtensions.cs` | Enables Swagger UI only when `IsDevelopment()` |
| Service registration | `Extensions/ServiceCollectionExtensions.cs` | Configures `AddSwaggerGen` with API versioning |
| Versioned OpenAPI docs | `Extensions/ConfigureSwaggerOptions.cs` | Generates a document per API version |
| Schema and examples | `Swagger/ConversionSchemaFilter.cs`, `Swagger/ConversionExamplesOperationFilter.cs` | Enriches request schemas and sample payloads |
| Descriptions | `Swagger/SwaggerDescriptions.cs` | API title, supported units, and usage notes |

XML comments on controllers and models are included in the generated OpenAPI document (`GenerateDocumentationFile` is enabled in the project file).

### Alternative ways to call the API

- **HTTP file:** `UnitConversion/UnitConversion.http` — use the **Send Request** link in Visual Studio or Cursor.
- **curl / PowerShell / fetch:** see [API overview](#api-overview) below.
- **Runtime discovery:** `GET /api/v1/categories` works in all environments (including Production and Docker).

## Run with Docker

```bash
docker compose up --build
```

The container exposes the API on `http://localhost:5053`.

## API overview

### Discover supported units

```http
GET /api/v1/categories
```

### Convert a value

```http
POST /api/v1/Conversion
Content-Type: application/json

{
  "category": "length",
  "fromUnit": "kilometers",
  "toUnit": "miles",
  "value": 1
}
```

Example response:

```json
{
  "category": "length",
  "originalValue": 1,
  "fromUnit": "kilometers",
  "toUnit": "miles",
  "convertedValue": 0.6214
}
```

Errors return RFC 7807 `ProblemDetails` with an `errorCode` extension.

## Project structure

```
UnitConversion/
  Controllers/     HTTP endpoints
  Services/        Application logic
  Converters/      Per-category conversion strategies
  Validators/      Request validation
  Middleware/      Global exception handling
  Constants/       Supported categories and units
  Swagger/         OpenAPI schema filters and API descriptions
  Properties/      Launch profiles (`launchSettings.json`)
UnitConversion.Tests/
  Api/             Integration tests
  Converters/      Unit tests for conversion logic
```

## Design decisions and trade-offs

### .NET 9

The project targets **.NET 9** to align with the challenge requirement for the latest stable ASP.NET Core runtime. **.NET 8 LTS** remains a reasonable alternative when long-term support is the primary concern.

### URL versioning (`/api/v1/`)

Versioning is included in the URL for clarity, cacheability, and straightforward client adoption. Header-based versioning was avoided to keep the public contract simple for this scope.

### Hardcoded conversion data

Units and factors are hardcoded in converter classes for the current scope. The `IUnitConverter` strategy pattern and dependency injection make it straightforward to add categories or load definitions from a database later.

### Swagger only in Development

Interactive API docs are disabled outside Development. Clients should use `GET /api/v1/categories` at runtime to discover supported units.

### Observability

- **Serilog** writes structured JSON logs to the console.
- **OpenTelemetry** exposes ASP.NET Core request metrics via a Prometheus endpoint at `/metrics`.

Full distributed tracing was omitted to keep operational overhead proportional to the API's complexity.

### Rate limiting and CORS

- Fixed-window rate limiting defaults to 100 requests per minute per client IP.
- CORS allowed origins are configuration-driven; Production defaults to no cross-origin access.

### Rounding

Converted values are rounded to four decimal places in the API response for consistent output.

## Configuration

| Setting | Location | Notes |
|---------|----------|-------|
| Log levels | `appsettings.json`, environment-specific files | Serilog reads from configuration |
| CORS origins | `Cors:AllowedOrigins` | Empty in Production by default |
| Rate limits | `RateLimiting:PermitLimit`, `WindowSeconds` | Applied to API controllers |
| Environment | `ASPNETCORE_ENVIRONMENT` | Controls Swagger, CORS defaults, log verbosity |

Use [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) or environment variables for local secrets. None are required for the current feature set.

## Testing

```bash
dotnet test
```

The test suite includes converter unit tests, validator tests, and integration tests for success paths, error handling, health checks, and category discovery.

## License

MIT — see [LICENSE](LICENSE).
