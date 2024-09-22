namespace Ecommerce.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an order is not found.
    /// </summary>
    public class OrderNotFoundException : Exception
    {
        /// <summary>
        /// Gets the ID of the order that was not found.
        /// </summary>
        public string? OrderId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNotFoundException"/> class with a default error message.
        /// </summary>
        public OrderNotFoundException() : base("Order not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public OrderNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public OrderNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderNotFoundException"/> class with a specified error message and order ID.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="orderId">The ID of the order that was not found.</param>
        public OrderNotFoundException(string message, string orderId) : base(message)
        {
            OrderId = orderId;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OrderNotFoundException"/> class with a formatted error message and the specified order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order that was not found.</param>
        /// <returns>A new instance of the <see cref="OrderNotFoundException"/> class.</returns>
        public static OrderNotFoundException FromId(string orderId)
        {
            return new OrderNotFoundException($"Order with ID '{orderId}' was not found.", orderId);
        }
    }
}