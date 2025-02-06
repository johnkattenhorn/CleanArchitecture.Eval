using Bookify.Domain.Apartments;

namespace Bookify.Api.Controllers.Apartments;

public sealed record AddApartmentRequest(
    string Name,
    string Description,
    string Country, 
    string State, 
    string ZipCode, 
    string City, 
    string Street,
    decimal PriceAmount, 
    string PriceCurrency, 
    decimal CleaningFeeAmount,
    string CleaningFeeCurrency,
    Amenity[] Amenities);
