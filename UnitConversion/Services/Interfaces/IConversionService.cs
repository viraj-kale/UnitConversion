using UnitConversion.Models.Requests;
using UnitConversion.Models.Responses;

namespace UnitConversion.Services.Interfaces;

public interface IConversionService
{
    ConversionResponse Convert(ConversionRequest request);
}
