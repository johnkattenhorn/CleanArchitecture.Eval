using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;

    public GetBookingQueryHandler(IApplicationDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        BookingResponse? booking = await _context.Bookings
            .Where(b => b.Id == request.BookingId)
            .Select(b => new BookingResponse
            {
                Id = b.Id,
                ApartmentId = b.ApartmentId,
                UserId = b.UserId,
                Status = (int)b.Status,
                PriceAmount = b.PriceForPeriod.Amount,
                PriceCurrency = b.PriceForPeriod.Currency.Code,
                CleaningFeeAmount = b.CleaningFee.Amount,
                CleaningFeeCurrency = b.CleaningFee.Currency.Code,
                AmenitiesUpChargeAmount = b.AmenitiesUpCharge.Amount,
                AmenitiesUpChargeCurrency = b.AmenitiesUpCharge.Currency.Code,
                TotalPriceAmount = b.TotalPrice.Amount,
                TotalPriceCurrency = b.TotalPrice.Currency.Code,
                DurationStart = b.Duration.Start,
                DurationEnd = b.Duration.End,
                CreatedOnUtc = b.CreatedOnUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (booking is null || booking.UserId != _userContext.UserId)
        {
            return Result.Failure<BookingResponse>(BookingErrors.NotFound);
        }

        return booking;
    }
}
