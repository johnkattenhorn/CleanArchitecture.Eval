using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotEligible = new(
        "Review.NotEligible",
        "The review is not eligible because the booking is not yet completed");

    public static readonly Error NotFound = new(
        "Review.NotFound",
        "The review with the specified identifier was not found");
}
