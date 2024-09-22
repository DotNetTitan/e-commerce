namespace Ecommerce.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a product is not found.
    /// </summary>
    public class ProductNotFoundException : Exception
    {
        /// <summary>
        /// Gets the ID of the product that was not found.
        /// </summary>
        public string? ProductId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a default error message.
        /// </summary>
        public ProductNotFoundException() : base("Product not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ProductNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ProductNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class with a specified error message and product ID.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="productId">The ID of the product that was not found.</param>
        public ProductNotFoundException(string message, string productId) : base(message)
        {
            ProductId = productId;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ProductNotFoundException"/> class with a formatted error message and the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product that was not found.</param>
        /// <returns>A new instance of the <see cref="ProductNotFoundException"/> class.</returns>
        public static ProductNotFoundException FromId(string productId)
        {
            return new ProductNotFoundException($"Product with ID '{productId}' was not found.", productId);
        }
    }
}