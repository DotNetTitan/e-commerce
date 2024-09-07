using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Features.CustomerManagement.EditCustomer
{
    public class EditCustomerResponse
    {
        public Guid CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Address? Address { get; set; }
    }
}