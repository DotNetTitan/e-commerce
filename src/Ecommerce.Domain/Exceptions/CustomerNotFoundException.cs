using System;
using System.Runtime.Serialization;

namespace Ecommerce.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a customer is not found.
    /// </summary>
    public class CustomerNotFoundException : Exception
    {
        /// <summary>
        /// Gets the ID of the customer that was not found.
        /// </summary>
        public string? CustomerId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerNotFoundException"/> class with a default error message.
        /// </summary>
        public CustomerNotFoundException() : base("Customer not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CustomerNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public CustomerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerNotFoundException"/> class with a specified error message and customer ID.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="customerId">The ID of the customer that was not found.</param>
        public CustomerNotFoundException(string message, string customerId) : base(message)
        {
            CustomerId = customerId;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CustomerNotFoundException"/> class with a formatted error message and the specified customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer that was not found.</param>
        /// <returns>A new instance of the <see cref="CustomerNotFoundException"/> class.</returns>
        public static CustomerNotFoundException FromId(Guid customerId)
        {
            return new CustomerNotFoundException($"Customer with ID '{customerId}' was not found.", customerId.ToString());
        }
    }
}