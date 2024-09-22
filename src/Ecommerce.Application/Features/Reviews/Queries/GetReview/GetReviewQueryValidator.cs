using FluentValidation;

namespace Ecommerce.Application.Features.Reviews.Queries.GetReview
{
    public class GetReviewQueryValidator : AbstractValidator<GetReviewQuery>
    {
        public GetReviewQueryValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Review ID is required.");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
        }
    }
}