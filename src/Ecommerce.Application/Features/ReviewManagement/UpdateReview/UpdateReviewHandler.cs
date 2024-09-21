using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ReviewManagement.UpdateReview
{
    public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, Result<UpdateReviewResponse>>
    {
        private readonly IReviewRepository _reviewRepository;

        public UpdateReviewHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<UpdateReviewResponse>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

            if (review == null)
            {
                return Result.Fail<UpdateReviewResponse>($"Review with ID {request.ReviewId} not found.");
            }

            if (review.ProductId != request.ProductId || review.CustomerId != request.CustomerId)
            {
                return Result.Fail<UpdateReviewResponse>("Invalid product or customer ID for this review.");
            }

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            var updatedReview = await _reviewRepository.UpdateAsync(review);

            if (updatedReview != null)
            {
                return Result.Ok(new UpdateReviewResponse
                {
                    ReviewId = updatedReview.ReviewId,
                    ProductId = updatedReview.ProductId,
                    CustomerId = updatedReview.CustomerId,
                    Rating = updatedReview.Rating,
                    Comment = updatedReview.Comment,
                    ReviewDate = updatedReview.ReviewDate
                });
            }

            return Result.Fail<UpdateReviewResponse>("Failed to update review");
        }
    }
}