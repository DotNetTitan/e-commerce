using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Api.DTOs.Customers
{
    public class EditCustomerDto
    {
        public Guid CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Address? Address { get; set; }
    }
}