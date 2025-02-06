using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Reviews.UpdateReview;

public sealed record UpdateReviewCommand(Guid ReviewId,int Rating, string Comment) : ICommand;
