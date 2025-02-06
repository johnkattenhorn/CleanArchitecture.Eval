using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentsQueryHandler
    : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private static readonly BookingStatus[] ActiveBookingStatuses =
    [
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
    ];

    private readonly IApplicationDbContext _context;

    public SearchApartmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return new List<ApartmentResponse>();
        }

        List<ApartmentResponse> apartments = await _context.Apartments
            .Where(a => !_context.Bookings.Any(b =>
                b.ApartmentId == a.Id &&
                b.Duration.Start <= request.EndDate &&
                b.Duration.End >= request.StartDate &&
                ActiveBookingStatuses.Contains(b.Status)))
            .Select(a => new ApartmentResponse
            {
                Id = a.Id,
                Name = a.Name.Value,
                Description = a.Description.Value,
                Price = a.Price.Amount,
                Currency = a.Price.Currency.Code,
                Address = new AddressResponse
                {
                    Country = a.Address.Country,
                    State = a.Address.State,
                    ZipCode = a.Address.ZipCode,
                    City = a.Address.City,
                    Street = a.Address.Street
                }
            })
            .ToListAsync(cancellationToken);

        return apartments;
    }
}
