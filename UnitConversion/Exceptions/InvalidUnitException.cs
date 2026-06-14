namespace UnitConversion.Exceptions;

public class InvalidUnitException : ApiException
{
    public InvalidUnitException(
        string unit,
        string category,
        IEnumerable<string> supportedUnits,
        string unitRole)
        : base(
            StatusCodes.Status400BadRequest,
            "INVALID_UNIT",
            "Invalid unit",
            $"Unit '{unit}' is not supported for category '{category}'.",
            new Dictionary<string, object?>
            {
                ["unit"] = unit,
                ["unitRole"] = unitRole,
                ["category"] = category,
                ["supportedUnits"] = supportedUnits.ToArray()
            })
    {
    }
}
