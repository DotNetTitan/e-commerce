using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            CustomerId = Guid.NewGuid();
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }

        public Guid CustomerId { get; set; }
        public required string IdentityId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Address? CustomerAddress { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}