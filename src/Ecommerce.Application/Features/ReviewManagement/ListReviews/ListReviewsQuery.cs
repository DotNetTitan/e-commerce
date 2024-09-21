using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ReviewManagement.ListReviews
{
    public class ListReviewsQuery : IRequest<Result<ListReviewsResponse>>
    {
        public required Guid ProductId { get; init; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}