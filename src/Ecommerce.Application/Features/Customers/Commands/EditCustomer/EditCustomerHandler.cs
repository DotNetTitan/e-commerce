﻿using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Features.Customers.Commands.EditCustomer
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
            customer.Email = request.Email;
            customer.CustomerAddress = request.Address;

            var updatedCustomer = await _customerRepository.UpdateAsync(customer);

            var response = new EditCustomerResponse
            {
                CustomerId = updatedCustomer.CustomerId,
                FirstName = updatedCustomer.FirstName!,
                LastName = updatedCustomer.LastName!,
                Address = updatedCustomer.CustomerAddress
            };

            return Result.Ok(response);
        }
    }

    public class EditCustomerCommand : IRequest<Result<EditCustomerResponse>>
    {
        public Guid CustomerId { get; init; }
        public required string Email { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public Address? Address { get; init; }
    }

    public class EditCustomerResponse
    {
        public Guid CustomerId { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public Address? Address { get; init; }
    }
}