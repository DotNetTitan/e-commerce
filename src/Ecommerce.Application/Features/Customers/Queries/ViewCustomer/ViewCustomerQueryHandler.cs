using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Features.Customers.Queries.ViewCustomer
{
    public class ViewCustomerQueryHandler : IRequestHandler<ViewCustomerQuery, Result<ViewCustomerQuerysResponse>>
    {
        private readonly ICustomerRepository _customerRepository;

        public ViewCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<ViewCustomerQuerysResponse>> Handle(ViewCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw CustomerNotFoundException.FromId(request.CustomerId);

            var response = new ViewCustomerQuerysResponse
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

    public class ViewCustomerQuery : IRequest<Result<ViewCustomerQuerysResponse>>
    {
        public Guid CustomerId { get; set; }
    }

    public class ViewCustomerQuerysResponse
    {
        public Guid CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Address? Address { get; set; }
    }
}