using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ReviewManagement.UpdateReview
{
    public class UpdateReviewCommand : IRequest<Result<UpdateReviewResponse>>
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
    }
}