using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Application.Apartments.AddApartments;

internal sealed class AddApartmentCommandHandler : ICommandHandler<AddApartmentCommand>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddApartmentCommandHandler(IUnitOfWork unitOfWork, IApartmentRepository apartmentRepository)
    {
        _unitOfWork = unitOfWork;
        _apartmentRepository = apartmentRepository;
    }

    public async Task<Result> Handle(AddApartmentCommand request, CancellationToken cancellationToken)
    {
        var adressResult = new Address(
            request.Country, 
            request.State, 
            request.ZipCode, 
            request.City, 
            request.Street);

        var priceResult = new Money(
            request.PriceAmount,
            Currency.FromCode(request.PriceCurrency));

        var cleaningFeeResult = new Money(
            request.CleaningFeeAmount,
            Currency.FromCode(request.CleaningFeeCurrency));

        Result<Apartment> apartmentResult = Apartment.Create(
            new Name(request.Name),
            new Description(request.Description),
            adressResult,
            priceResult,
            cleaningFeeResult,
            request.Amenities);

        if (apartmentResult.IsFailure)
        {
            return Result.Failure(apartmentResult.Error);
        }

        _apartmentRepository.Add(apartmentResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
