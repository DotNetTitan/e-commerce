namespace Ecommerce.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a user is not found.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Gets the username of the user that was not found.
        /// </summary>
        public string? UserName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a default error message.
        /// </summary>
        public UserNotFoundException() : base("User not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UserNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error message and username.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="userName">The username of the user that was not found.</param>
        public UserNotFoundException(string message, string userName) : base(message)
        {
            UserName = userName;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserNotFoundException"/> class with a formatted error message and the specified username.
        /// </summary>
        /// <param name="userName">The username of the user that was not found.</param>
        /// <returns>A new instance of the <see cref="UserNotFoundException"/> class.</returns>
        public static UserNotFoundException FromUserName(string userName)
        {
            return new UserNotFoundException($"User with UserName '{userName}' was not found.", userName);
        }
    }
}