using FluentValidation;

namespace Ecommerce.Application.Features.ReviewManagement.DeleteReview
{
    public class DeleteReviewValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID is required.");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
        }
    }
}