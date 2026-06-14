namespace UnitConversion.Exceptions;

public class ValidationApiException : ApiException
{
    public ValidationApiException(
        IDictionary<string, string[]> errors,
        string detail = "One or more validation errors occurred.")
        : base(
            StatusCodes.Status400BadRequest,
            "VALIDATION_ERROR",
            "Validation failed",
            detail,
            new Dictionary<string, object?> { ["errors"] = errors })
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}
