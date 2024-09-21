namespace Ecommerce.Application.Features.ReviewManagement.DeleteReview
{
    public class DeleteReviewResponse
    {
        public required Guid ReviewId { get; init; }
        public required bool IsDeleted { get; init; }
    }
}