using UnitConversion.Models.Requests;
using UnitConversion.Models.Responses;

public interface IConversionService
{
    ConversionResponse Convert(ConversionRequest request);
}