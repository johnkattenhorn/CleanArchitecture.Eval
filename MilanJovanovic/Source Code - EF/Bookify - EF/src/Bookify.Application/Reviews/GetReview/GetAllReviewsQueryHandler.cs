using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Reviews.GetReview;

internal sealed class GetAllReviewsQueryHandler : IQueryHandler<GetAllReviewsQuery, IReadOnlyList<ReviewResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyList<ReviewResponse>>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
    {
        List<ReviewResponse> reviews = await _context.Reviews
            .Select(r => new ReviewResponse
            {
                Id = r.Id,
                ApartmentId = r.ApartmentId,
                BookingId = r.BookingId,
                UserId = r.UserId,
                Rating = r.Rating.Value,
                Comment = r.Comment.Value,
                CreatedOnUtc = r.CreatedOnUtc
            })
            .ToListAsync(cancellationToken);

        return reviews;
    }
}
