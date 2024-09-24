using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Api.DTOs.Customers
{
    public class EditCustomerDto
    {
        public Guid CustomerId { get; init; }
        public required string Email { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public Address? Address { get; init; }
    }
}