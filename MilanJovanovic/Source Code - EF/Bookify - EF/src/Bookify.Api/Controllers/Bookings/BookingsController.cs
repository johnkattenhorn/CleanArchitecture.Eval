using Asp.Versioning;
using Bookify.Application.Bookings.CompleteBooking;
using Bookify.Application.Bookings.CancelBooking;
using Bookify.Application.Bookings.ConfirmBooking;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.RejectBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bookify.Application.Bookings.GetBookings;

namespace Bookify.Api.Controllers.Bookings;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/bookings")]
public class BookingsController : ControllerBase
{
    private readonly ISender _sender;

    public BookingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        Result<Application.Bookings.GetBooking.BookingResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings(CancellationToken cancellationToken)
    {
        var query = new GetAllBookingsQuery();

        Result<IReadOnlyList<Application.Bookings.GetBookings.BookingResponse>> result =
            await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteBooking(Guid id, CancellationToken cancellationToken)
    {
        var update = new CompleteBookingCommand(id);

        Result result = await _sender.Send(update, cancellationToken);

        if(result.IsFailure)
        
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, CancellationToken cancellationToken)
    {
        var update = new CancelBookingCommand(id);

        Result result = await _sender.Send(update, cancellationToken);

        if(result.IsFailure)
        
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> ConfirmBooking(Guid id, CancellationToken cancellationToken)
    {
        var update = new ConfirmBookingCommand(id);

        Result result = await _sender.Send(update, cancellationToken);

        if(result.IsFailure)
        
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("{id}/reject")]
    public async Task<IActionResult> RejectBooking(Guid id, CancellationToken cancellationToken)
    {
        var update = new RejectBookingCommand(id);

        Result result = await _sender.Send(update, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
