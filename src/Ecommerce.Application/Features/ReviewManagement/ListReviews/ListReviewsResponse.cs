namespace Ecommerce.Application.Features.ReviewManagement.ListReviews
{
    public class ListReviewsResponse
    {
        public required Guid ProductId { get; init; }
        public required List<ReviewDetails> Reviews { get; init; }
        public required int TotalCount { get; init; }
        public required int PageNumber { get; init; }
        public required int PageSize { get; init; }
    }

    public class ReviewDetails
    {
        public required Guid ReviewId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public required DateTime ReviewDate { get; init; }
    }
}