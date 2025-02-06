using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Reviews.GetReview;

public sealed record GetAllReviewsQuery : IQuery<IReadOnlyList<ReviewResponse>>;
