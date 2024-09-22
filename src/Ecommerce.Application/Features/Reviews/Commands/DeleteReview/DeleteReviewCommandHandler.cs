using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result<DeleteReviewResponse>>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<DeleteReviewResponse>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId);

            if (review == null)
            {
                return Result.Fail<DeleteReviewResponse>($"Review with ID {request.ReviewId} not found.");
            }

            if (review.ProductId != request.ProductId || review.CustomerId != request.CustomerId)
            {
                return Result.Fail<DeleteReviewResponse>("Invalid product or customer ID for this review.");
            }

            var isDeleted = await _reviewRepository.DeleteAsync(review);

            return Result.Ok(new DeleteReviewResponse
            {
                ReviewId = request.ReviewId,
                IsDeleted = isDeleted
            });
        }
    }

    public class DeleteReviewCommand : IRequest<Result<DeleteReviewResponse>>
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
    }

    public class DeleteReviewResponse
    {
        public required Guid ReviewId { get; init; }
        public required bool IsDeleted { get; init; }
    }
}