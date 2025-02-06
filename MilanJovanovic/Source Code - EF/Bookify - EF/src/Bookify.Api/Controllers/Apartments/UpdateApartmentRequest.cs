using Bookify.Domain.Apartments;

namespace Bookify.Api.Controllers.Apartments;

public sealed record UpdateApartmentRequest(
    decimal PriceAmount,
    string PriceAmountCurrency,
    decimal CleaningFeeAmount,
    string CleaningFeeCurrency,
    Amenity[] Amenities);
