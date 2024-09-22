using FluentValidation;

namespace Ecommerce.Application.Features.Customers.Queries.ViewCustomer
{
    public class ViewCustomerQueryValidator : AbstractValidator<ViewCustomerQuery>
    {
        public ViewCustomerQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
        }
    }
}