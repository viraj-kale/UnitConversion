namespace UnitConversion.Exceptions;

public class ApiException : Exception
{
    public ApiException(
        int statusCode,
        string errorCode,
        string title,
        string detail,
        IReadOnlyDictionary<string, object?>? extensions = null)
        : base(detail)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Title = title;
        Extensions = extensions;
    }

    public int StatusCode { get; }

    public string ErrorCode { get; }

    public string Title { get; }

    public IReadOnlyDictionary<string, object?>? Extensions { get; }
}
