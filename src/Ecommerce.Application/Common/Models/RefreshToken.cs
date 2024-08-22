namespace Ecommerce.Application.Common.Models
{
    /// <summary>
    /// Represents a refresh token used for authentication.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshToken"/> class.
        /// </summary>
        public RefreshToken()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the unique identifier of the refresh token.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the refresh token.
        /// </summary>
        public required string IdentityId { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the refresh token.
        /// </summary>
        public required DateTime ExpirationDate { get; set; }
    }
}