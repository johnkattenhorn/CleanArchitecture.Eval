using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Apartments;

namespace Bookify.Application.Apartments.UpdateApartments;

public sealed record UpdateApartmentCommand(
    Guid ApartmentId,
    decimal PriceAmount,
    string PriceAmountCurrency,
    decimal CleaningFeeAmount,
    string CleaningFeeCurrency,
    Amenity[] Amenities) : ICommand;
