using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Application.Apartments.UpdateApartments;

internal sealed class UpdateApartmentCommandHandler : ICommandHandler<UpdateApartmentCommand>
{
    public readonly IApartmentRepository _apartmentRepository;
    public readonly IUnitOfWork _unitOfWork;

    public UpdateApartmentCommandHandler(
        IApartmentRepository apartmentRepository,
        IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
    {
        Apartment? apartment= await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);

        if (apartment is null)
        {
            return Result.Failure(ApartmentErrors.NotFound);
        }

        var priceResult = new Money(
            request.PriceAmount,
            Currency.FromCode(request.PriceAmountCurrency));

        var cleaningFeeResult = new Money(
            request.CleaningFeeAmount,
            Currency.FromCode(request.PriceAmountCurrency));

        apartment.Update(priceResult, cleaningFeeResult, request.Amenities);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
