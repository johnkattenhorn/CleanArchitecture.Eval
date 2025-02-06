using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Reviews.DeleteReview;

public sealed record DeleteReviewCommand(Guid ReviewId) : ICommand;
