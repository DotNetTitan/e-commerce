using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ReviewManagement.GetReview
{
    public class GetReviewQuery : IRequest<Result<GetReviewResponse>>
    {
        public required Guid ProductId { get; init; }
        public required Guid ReviewId { get; init; }
    }
}