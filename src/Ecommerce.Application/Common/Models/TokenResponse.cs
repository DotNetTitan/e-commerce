namespace Ecommerce.Application.Common.Models
{
    /// <summary>
    /// Represents a token response containing an access token and a refresh token.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}