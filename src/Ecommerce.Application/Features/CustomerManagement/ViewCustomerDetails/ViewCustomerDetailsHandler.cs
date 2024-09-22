using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Features.CustomerManagement.ViewCustomerDetails
{
    public class ViewCustomerDetailsHandler : IRequestHandler<ViewCustomerDetailsQuery, Result<ViewCustomerDetailsResponse>>
    {
        private readonly ICustomerRepository _customerRepository;

        public ViewCustomerDetailsHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<ViewCustomerDetailsResponse>> Handle(ViewCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw CustomerNotFoundException.FromId(request.CustomerId);

            var response = new ViewCustomerDetailsResponse
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = new Address
                (
                    customer.CustomerAddress?.Building ?? string.Empty,
                    customer.CustomerAddress?.Street ?? string.Empty,
                    customer.CustomerAddress?.PostalCode ?? string.Empty,
                    customer.CustomerAddress?.City ?? string.Empty,
                    customer.CustomerAddress?.State ?? string.Empty,
                    customer.CustomerAddress?.Country ?? string.Empty
                )
            };

            return Result.Ok(response);
        }
    }
}