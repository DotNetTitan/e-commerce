using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ReviewManagement.ListReviews
{
    public class ListReviewsHandler : IRequestHandler<ListReviewsQuery, Result<ListReviewsResponse>>
    {
        private readonly IReviewRepository _reviewRepository;

        public ListReviewsHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<ListReviewsResponse>> Handle(ListReviewsQuery request, CancellationToken cancellationToken)
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

            var response = new ListReviewsResponse
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
}