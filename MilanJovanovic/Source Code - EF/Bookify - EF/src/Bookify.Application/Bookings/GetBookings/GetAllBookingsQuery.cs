using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Bookings.GetBookings;

public sealed record GetAllBookingsQuery : IQuery<IReadOnlyList<BookingResponse>>;
