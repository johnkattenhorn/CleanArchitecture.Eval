using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Reviews;

namespace Bookify.Application.Reviews.DeleteReview;

internal sealed class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        Review? review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review == null)
        {
            return Result.Failure(ReviewErrors.NotFound);
        }

        _reviewRepository.Remove(review);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
