using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ReviewManagement.DeleteReview
{
    public class DeleteReviewCommand : IRequest<Result<DeleteReviewResponse>>
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
    }
}