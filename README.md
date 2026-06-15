# Unit Conversion API

ASP.NET Core Web API for converting numeric values between units of measurement across length, weight, temperature, volume, area, speed, and time.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Optional: [Docker](https://www.docker.com/) for containerized runs

## Run locally

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project UnitConversion
```

The API listens on `http://localhost:5053` by default.

- Swagger UI (Development only): `http://localhost:5053/swagger`
- Health check: `GET /health`
- Readiness check: `GET /health/ready`
- Prometheus metrics: `GET /metrics`

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
