using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.ValueObjects;
using FluentValidation;

namespace Ecommerce.Application.Features.Customers.Commands.EditCustomer
{
    public class EditCustomerValidator : AbstractValidator<EditCustomerCommand>
    {
        public EditCustomerValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50).WithMessage("First name is required and should not exceed 50 characters.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50).WithMessage("Last name is required and should not exceed 50 characters.");
            RuleFor(x => x.Address)
                .SetValidator(new AddressValidator()!)
                .When(x => x.Address != null)
                .WithMessage("Invalid address.");

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address)
                    .SetValidator(new AddressValidator()!);
            }).Otherwise(() =>
            {
                RuleFor(x => x.Address).Null();
            });
        }
    }

    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Building).NotEmpty().MaximumLength(100).WithMessage("Building is required and should not exceed 100 characters.");
            RuleFor(x => x.Street).NotEmpty().MaximumLength(100).WithMessage("Street is required and should not exceed 100 characters.");
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20).WithMessage("Postal code is required and should not exceed 20 characters.");
            RuleFor(x => x.City).NotEmpty().MaximumLength(50).WithMessage("City is required and should not exceed 50 characters.");
            RuleFor(x => x.State).NotEmpty().MaximumLength(50).WithMessage("State is required and should not exceed 50 characters.");
            RuleFor(x => x.Country).NotEmpty().MaximumLength(50).WithMessage("Country is required and should not exceed 50 characters.");
        }
    }
}