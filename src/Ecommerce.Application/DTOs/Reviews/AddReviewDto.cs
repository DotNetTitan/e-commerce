namespace Ecommerce.Application.DTOs.Reviews
{
    public class AddReviewDto
    {
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public DateTime ReviewDate { get; init; }
    }
}