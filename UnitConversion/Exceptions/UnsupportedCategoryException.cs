using UnitConversion.Constants;

namespace UnitConversion.Exceptions;

public class UnsupportedCategoryException : ApiException
{
    public UnsupportedCategoryException(string category)
        : base(
            StatusCodes.Status400BadRequest,
            "UNSUPPORTED_CATEGORY",
            "Unsupported category",
            $"Category '{category}' is not supported.",
            new Dictionary<string, object?>
            {
                ["category"] = category,
                ["supportedCategories"] = SupportedUnits.All
                    .Select(info => info.Name)
                    .ToArray()
            })
    {
    }
}
