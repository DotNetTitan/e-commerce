using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a customer in the e-commerce system.
    /// </summary>
    public class Customer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the identity identifier for the customer.
        /// </summary>
        public required string IdentityId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the address of the customer.
        /// </summary>
        public Address? CustomerAddress { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart associated with the customer.
        /// </summary>
        public ShoppingCart? ShoppingCart { get; set; }

        /// <summary>
        /// Gets or sets the collection of orders placed by the customer.
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the collection of reviews written by the customer.
        /// </summary>
        public ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// Initializes a new instance of the Customer class.
        /// </summary>
        public Customer()
        {
            CustomerId = Guid.NewGuid();
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }
    }
}