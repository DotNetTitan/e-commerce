using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.CustomerManagement.EditCustomer
{
    public class EditCustomerHandler : IRequestHandler<EditCustomerCommand, Result<EditCustomerResponse>>
    {
        private readonly ICustomerRepository _customerRepository;

        public EditCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<EditCustomerResponse>> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw CustomerNotFoundException.FromId(request.CustomerId);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.CustomerAddress = request.Address != null ? CreateAddress(request.Address) : customer.CustomerAddress;

            var updatedCustomer = await _customerRepository.UpdateAsync(customer);

            return Result.Ok(CreateEditCustomerResponse(updatedCustomer));

        }

        private static Address CreateAddress(Address address)
        {
            return new Address(
                address.Building ?? string.Empty,
                address.Street ?? string.Empty,
                address.PostalCode ?? string.Empty,
                address.City ?? string.Empty,
                address.State ?? string.Empty,
                address.Country ?? string.Empty
            );
        }

        private static EditCustomerResponse CreateEditCustomerResponse(Customer updatedCustomer)
        {
            return new EditCustomerResponse
            {
                CustomerId = updatedCustomer.CustomerId,
                FirstName = updatedCustomer.FirstName!,
                LastName = updatedCustomer.LastName!,
                Address = new Address
                (
                    updatedCustomer.CustomerAddress?.Building ?? string.Empty,
                    updatedCustomer.CustomerAddress?.Street ?? string.Empty,
                    updatedCustomer.CustomerAddress?.PostalCode ?? string.Empty,
                    updatedCustomer.CustomerAddress?.City ?? string.Empty,
                    updatedCustomer.CustomerAddress?.State ?? string.Empty,
                    updatedCustomer.CustomerAddress?.Country ?? string.Empty
                )
            };
        }
    }
}