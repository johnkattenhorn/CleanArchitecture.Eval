using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Apartments;

namespace Bookify.Application.Apartments.AddApartments;

public sealed record AddApartmentCommand(
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
    Amenity[] Amenities) : ICommand;
