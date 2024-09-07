using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Application.Features.CustomerManagement.ViewCustomerDetails
{
    public class ViewCustomerDetailsResponse
    {
        public Guid CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Address? Address { get; set; }
    }
}