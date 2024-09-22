using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Reviews.Queries.GetReview
{
    public class GetReviewQueryHandler : IRequestHandler<GetReviewQuery, Result<GetReviewResponse>>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetReviewQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<GetReviewResponse>> Handle(GetReviewQuery request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

            if (review == null || review.ProductId != request.ProductId)
            {
                return Result.Fail<GetReviewResponse>($"Review with ID {request.ReviewId} not found for product {request.ProductId}.");
            }

            return Result.Ok(new GetReviewResponse
            {
                ReviewId = review.ReviewId,
                ProductId = review.ProductId,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            });
        }
    }

    public class GetReviewQuery : IRequest<Result<GetReviewResponse>>
    {
        public required Guid ProductId { get; init; }
        public required Guid ReviewId { get; init; }
    }

    public class GetReviewResponse
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public required DateTime ReviewDate { get; init; }
    }
}