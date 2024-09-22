using FluentValidation;

namespace Ecommerce.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID is required.");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
        }
    }
}