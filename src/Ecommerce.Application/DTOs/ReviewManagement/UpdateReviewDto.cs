namespace Ecommerce.Application.DTOs.ReviewManagement
{
    public class UpdateReviewDto
    {
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
    }
}