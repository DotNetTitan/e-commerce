namespace Ecommerce.Domain.Entities
{
    /// <summary>
    /// Represents a product review in the e-commerce system.
    /// </summary>
    public class Review : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public Guid ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier associated with the review.
        /// </summary>
        public required Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product associated with the review.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier associated with the review.
        /// </summary>
        public required Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer associated with the review.
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// Gets or sets the comment for the review.
        /// </summary>
        public required string Comment { get; set; }

        /// <summary>
        /// Gets or sets the rating for the review.
        /// </summary>
        public required int Rating { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the review was submitted.
        /// </summary>
        public required DateTime ReviewDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the Review class.
        /// </summary>
        public Review()
        {
            ReviewId = Guid.NewGuid();
        }
    }
}