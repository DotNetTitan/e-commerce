using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Reviews.Queries.ListReviews
{
    public class ListReviewsQueryHandler : IRequestHandler<ListReviewsQuery, Result<ListReviewsQueryResponse>>
    {
        private readonly IReviewRepository _reviewRepository;

        public ListReviewsQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<ListReviewsQueryResponse>> Handle(ListReviewsQuery request, CancellationToken cancellationToken)
        {
            var (reviews, totalCount) = await _reviewRepository.GetReviewsByProductIdAsync(
                request.ProductId,
                request.PageNumber,
                request.PageSize
            );

            var reviewDetails = reviews.ConvertAll(r => new ReviewDetails
            {
                ReviewId = r.ReviewId,
                CustomerId = r.CustomerId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            });

            var response = new ListReviewsQueryResponse
            {
                ProductId = request.ProductId,
                Reviews = reviewDetails,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result.Ok(response);
        }
    }

    public class ListReviewsQuery : IRequest<Result<ListReviewsQueryResponse>>
    {
        public required Guid ProductId { get; init; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ListReviewsQueryResponse
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