# Unit Conversion API

A REST API built with **ASP.NET Core 8** that converts numeric values between units within supported categories (length, weight, temperature, volume, area, speed, and time).

All endpoints return **JSON** and follow [RFC 7807 Problem Details](https://tools.ietf.org/html/rfc7807) for error responses.

---

## Table of Contents

- [Getting Started](#getting-started)
- [Base URL](#base-url)
- [Interactive Documentation (Swagger)](#interactive-documentation-swagger)
- [API Reference](#api-reference)
  - [Convert Units](#convert-units)
- [Request Schema](#request-schema)
- [Response Schema](#response-schema)
- [Supported Categories and Units](#supported-categories-and-units)
- [Error Responses](#error-responses)
- [Usage Examples](#usage-examples)
- [Behavior Notes](#behavior-notes)

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run the API

From the solution directory:

```bash
cd UnitConversion
dotnet run --project UnitConversion
```

By default, the API starts with the **https** launch profile:

| Protocol | URL |
|----------|-----|
| HTTPS | `https://localhost:7113` |
| HTTP | `http://localhost:5053` |

To use HTTP only:

```bash
dotnet run --project UnitConversion --launch-profile http
```

### Run Tests

```bash
dotnet test
```

---

## Base URL

When running locally with the default profile:

```
https://localhost:7113
```

All API routes are prefixed with `/api`.

---

## Interactive Documentation (Swagger)

Swagger UI is enabled in all environments.

| Resource | URL |
|----------|-----|
| Swagger UI | `https://localhost:7113/swagger` |
| OpenAPI JSON | `https://localhost:7113/swagger/v1/swagger.json` |

Swagger includes example request bodies for every supported category.

---

## API Reference

### Convert Units

Converts a numeric value from one unit to another within a single category.

| | |
|---|---|
| **Method** | `POST` |
| **Path** | `/api/Conversion` |
| **Content-Type** | `application/json` |
| **Authentication** | None |

#### Request Body Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `category` | `string` | Yes | The conversion category. Must be one of: `length`, `weight`, `temperature`, `volume`, `area`, `speed`, `time`. Case-insensitive. |
| `fromUnit` | `string` | Yes | The source unit to convert **from**. Must be valid for the selected `category`. Case-insensitive. |
| `toUnit` | `string` | Yes | The target unit to convert **to**. Must be valid for the selected `category`. Case-insensitive. |
| `value` | `number` | Yes | The numeric value to convert. Must be a finite number (not `null`, `NaN`, or `Infinity`). |

#### Success Response

| Status | Description |
|--------|-------------|
| `200 OK` | Conversion completed successfully. |

#### Error Responses

| Status | Description |
|--------|-------------|
| `400 Bad Request` | Missing/invalid fields, unsupported category, invalid unit, malformed JSON, or non-finite `value`. |
| `500 Internal Server Error` | Unexpected server error. |

---

## Request Schema

```json
{
  "category": "string",
  "fromUnit": "string",
  "toUnit": "string",
  "value": 0
}
```

### Example Request

```json
{
  "category": "length",
  "fromUnit": "kilometers",
  "toUnit": "miles",
  "value": 1
}
```

---

## Response Schema

### `200 OK` — ConversionResponse

| Field | Type | Description |
|-------|------|-------------|
| `originalValue` | `number` | The input value before conversion (echoed from the request). |
| `fromUnit` | `string` | The source unit (echoed from the request). |
| `toUnit` | `string` | The target unit (echoed from the request). |
| `convertedValue` | `number` | The converted result, rounded to **4 decimal places**. |

### Example Success Response

```json
{
  "originalValue": 1,
  "fromUnit": "kilometers",
  "toUnit": "miles",
  "convertedValue": 0.6214
}
```

---

## Supported Categories and Units

Unit names are **case-insensitive**. For most categories, **singular and plural** forms are accepted (for example, `meter` / `meters`, `foot` / `feet`).

Temperature units accept only the canonical names listed below (no plural aliases).

### Length (`category: "length"`)

Distance and dimension measurements. Base unit: **meter**.

| Unit | Accepted aliases |
|------|------------------|
| `meter` | `meters` |
| `kilometer` | `kilometers` |
| `foot` | `feet` |
| `mile` | `miles` |

### Weight (`category: "weight"`)

Mass measurements. Base unit: **gram**.

| Unit | Accepted aliases |
|------|------------------|
| `gram` | `grams` |
| `kilogram` | `kilograms` |
| `pound` | `pounds` |

### Temperature (`category: "temperature"`)

Temperature scales using offset conversions (not simple multiplication factors).

| Unit | Accepted aliases |
|------|------------------|
| `celsius` | — |
| `fahrenheit` | — |
| `kelvin` | — |

### Volume (`category: "volume"`)

Liquid and capacity measurements. Uses the **US gallon**. Base unit: **liter**.

| Unit | Accepted aliases |
|------|------------------|
| `liter` | `liters` |
| `milliliter` | `milliliters` |
| `gallon` | `gallons` |
| `cup` | `cups` |

### Area (`category: "area"`)

Surface and land area measurements. Base unit: **squaremeter**.

| Unit | Accepted aliases |
|------|------------------|
| `squaremeter` | `squaremeters` |
| `squarefoot` | `squarefeet` |
| `acre` | `acres` |
| `hectare` | `hectares` |

### Speed (`category: "speed"`)

Rate of motion. Base unit: **meterpersecond**.

| Unit | Accepted aliases |
|------|------------------|
| `meterpersecond` | `meterspersecond` |
| `kilometerperhour` | `kilometersperhour` |
| `mileperhour` | `milesperhour` |

### Time (`category: "time"`)

Duration measurements. Base unit: **second**.

| Unit | Accepted aliases |
|------|------------------|
| `second` | `seconds` |
| `minute` | `minutes` |
| `hour` | `hours` |
| `day` | `days` |

---

## Error Responses

Errors are returned as JSON [Problem Details](https://tools.ietf.org/html/rfc7807) objects.

### Common Problem Details Fields

| Field | Type | Description |
|-------|------|-------------|
| `type` | `string` | URI reference identifying the problem type. |
| `title` | `string` | Short, human-readable summary. |
| `status` | `number` | HTTP status code. |
| `detail` | `string` | Human-readable explanation of the error. |
| `instance` | `string` | Request path that caused the error. |
| `errorCode` | `string` | Application-specific error code (in `extensions`). |

### Error Codes

| HTTP Status | `errorCode` | When it occurs |
|-------------|-------------|----------------|
| `400` | `VALIDATION_ERROR` | Required fields missing, empty values, non-finite `value`, or model binding failure. |
| `400` | `UNSUPPORTED_CATEGORY` | The `category` value is not supported. |
| `400` | `INVALID_UNIT` | `fromUnit` or `toUnit` is not valid for the given category. |
| `400` | `BAD_REQUEST` | Other bad request scenarios (e.g. invalid argument). |
| `500` | `INTERNAL_SERVER_ERROR` | Unhandled server exception. |

---

### `400` — Validation Error (`VALIDATION_ERROR`)

Returned when required fields are missing, values are empty/whitespace, or `value` is not a finite number.

**Example — missing fields:**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/Conversion",
  "errorCode": "VALIDATION_ERROR",
  "errors": {
    "Category": ["Category is required."],
    "FromUnit": ["FromUnit is required."],
    "ToUnit": ["ToUnit is required."],
    "Value": ["Value is required."]
  }
}
```

**Example — non-finite value:**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "errorCode": "VALIDATION_ERROR",
  "errors": {
    "value": ["Value must be a finite number."]
  }
}
```

---

### `400` — Unsupported Category (`UNSUPPORTED_CATEGORY`)

Returned when `category` does not match any supported conversion type.

**Example:**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Unsupported category",
  "status": 400,
  "detail": "Category 'distance' is not supported.",
  "instance": "/api/Conversion",
  "errorCode": "UNSUPPORTED_CATEGORY",
  "category": "distance",
  "supportedCategories": [
    "length",
    "weight",
    "temperature",
    "volume",
    "area",
    "speed",
    "time"
  ]
}
```

---

### `400` — Invalid Unit (`INVALID_UNIT`)

Returned when `fromUnit` or `toUnit` is not valid for the selected category.

| Extension field | Type | Description |
|-----------------|------|-------------|
| `unit` | `string` | The invalid unit that was provided. |
| `unitRole` | `string` | Which field failed: `"fromUnit"` or `"toUnit"`. |
| `category` | `string` | The category from the request. |
| `supportedUnits` | `string[]` | Canonical units supported for that category. |

**Example:**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid unit",
  "status": 400,
  "detail": "Unit 'yards' is not supported for category 'length'.",
  "instance": "/api/Conversion",
  "errorCode": "INVALID_UNIT",
  "unit": "yards",
  "unitRole": "fromUnit",
  "category": "length",
  "supportedUnits": ["meter", "kilometer", "foot", "mile"]
}
```

---

### `500` — Internal Server Error (`INTERNAL_SERVER_ERROR`)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal server error",
  "status": 500,
  "detail": "An unexpected error occurred while processing the request.",
  "instance": "/api/Conversion",
  "errorCode": "INTERNAL_SERVER_ERROR"
}
```

---

## Usage Examples

### cURL

**Length — kilometers to miles:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"length\",\"fromUnit\":\"kilometers\",\"toUnit\":\"miles\",\"value\":1}"
```

**Temperature — Celsius to Fahrenheit:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"temperature\",\"fromUnit\":\"celsius\",\"toUnit\":\"fahrenheit\",\"value\":100}"
```

**Weight — kilograms to pounds:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"weight\",\"fromUnit\":\"kilograms\",\"toUnit\":\"pounds\",\"value\":2.5}"
```

**Volume — gallons to liters:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"volume\",\"fromUnit\":\"gallons\",\"toUnit\":\"liters\",\"value\":1}"
```

**Area — acres to square feet:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"area\",\"fromUnit\":\"acres\",\"toUnit\":\"squarefeet\",\"value\":1}"
```

**Speed — miles per hour to kilometers per hour:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"speed\",\"fromUnit\":\"milesperhour\",\"toUnit\":\"kilometersperhour\",\"value\":60}"
```

**Time — hours to minutes:**

```bash
curl -X POST "https://localhost:7113/api/Conversion" \
  -H "Content-Type: application/json" \
  -d "{\"category\":\"time\",\"fromUnit\":\"hours\",\"toUnit\":\"minutes\",\"value\":2}"
```

---

### PowerShell

```powershell
$body = @{
    category = "length"
    fromUnit = "kilometers"
    toUnit   = "miles"
    value    = 1
} | ConvertTo-Json

Invoke-RestMethod `
    -Uri "https://localhost:7113/api/Conversion" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

---

### JavaScript (fetch)

```javascript
const response = await fetch("https://localhost:7113/api/Conversion", {
  method: "POST",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({
    category: "length",
    fromUnit: "kilometers",
    toUnit: "miles",
    value: 1
  })
});

const data = await response.json();

if (response.ok) {
  console.log(data.convertedValue); // 0.6214
} else {
  console.error(data.errorCode, data.detail);
}
```

---

### C# (HttpClient)

```csharp
using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7113") };

var request = new
{
    category = "length",
    fromUnit = "kilometers",
    toUnit = "miles",
    value = 1
};

var response = await client.PostAsJsonAsync("/api/Conversion", request);
response.EnsureSuccessStatusCode();

var result = await response.Content.ReadFromJsonAsync<ConversionResponse>();
Console.WriteLine(result.ConvertedValue);
```

---

## Behavior Notes

| Topic | Behavior |
|-------|----------|
| **Rounding** | Results are rounded to **4 decimal places** using standard `Math.Round`. |
| **Case sensitivity** | `category`, `fromUnit`, and `toUnit` are matched case-insensitively. |
| **Unit aliases** | Most categories accept singular and plural forms (see [Supported Categories and Units](#supported-categories-and-units)). |
| **Same-unit conversion** | Converting a unit to itself (e.g. `meters` → `meters`) is valid and returns the same value (rounded). |
| **Cross-category units** | Units from one category cannot be used in another (e.g. `pounds` in a `length` request returns `INVALID_UNIT`). |
| **Volume gallon** | `gallon` refers to the **US liquid gallon** (3.78541 liters). |
| **HTTPS** | The default launch profile redirects HTTP to HTTPS. Use `-k` with cURL if using a self-signed development certificate. |

---

## Project Structure

```
UnitConversion/
├── UnitConversion.sln
├── UnitConversion/
│   ├── Controllers/
│   │   └── ConversionController.cs    # API endpoints
│   ├── Models/
│   │   ├── Requests/ConversionRequest.cs
│   │   └── Responses/ConversionResponse.cs
│   ├── Services/                      # Business logic
│   ├── Converters/                    # Per-category conversion logic
│   ├── Validators/                    # Request validation
│   ├── Middleware/                    # Global exception handling
│   └── Swagger/                       # OpenAPI documentation helpers
└── UnitConversion.Tests/              # Unit and integration tests
```
