using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result<UpdateReviewCommandResponse>>
    {
        private readonly IReviewRepository _reviewRepository;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<UpdateReviewCommandResponse>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

            if (review == null)
            {
                return Result.Fail<UpdateReviewCommandResponse>($"Review with ID {request.ReviewId} not found.");
            }

            if (review.ProductId != request.ProductId || review.CustomerId != request.CustomerId)
            {
                return Result.Fail<UpdateReviewCommandResponse>("Invalid product or customer ID for this review.");
            }

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            var updatedReview = await _reviewRepository.UpdateAsync(review);

            if (updatedReview != null)
            {
                return Result.Ok(new UpdateReviewCommandResponse
                {
                    ReviewId = updatedReview.ReviewId,
                    ProductId = updatedReview.ProductId,
                    CustomerId = updatedReview.CustomerId,
                    Rating = updatedReview.Rating,
                    Comment = updatedReview.Comment,
                    ReviewDate = updatedReview.ReviewDate
                });
            }

            return Result.Fail<UpdateReviewCommandResponse>("Failed to update review");
        }
    }

    public class UpdateReviewCommand : IRequest<Result<UpdateReviewCommandResponse>>
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
    }

    public class UpdateReviewCommandResponse
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public required DateTime ReviewDate { get; init; }
    }
}