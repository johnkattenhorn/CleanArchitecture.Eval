using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Reviews;

namespace Bookify.Application.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        Review? review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
        {
            return Result.Failure(ReviewErrors.NotFound);
        }

        Result<Rating> ratingResult = Rating.Create(request.Rating);

        if (ratingResult.IsFailure)
        {
            return Result.Failure(ratingResult.Error);
        }

        review.Update(ratingResult.Value, new Comment(request.Comment));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
