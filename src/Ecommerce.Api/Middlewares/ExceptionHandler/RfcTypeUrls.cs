namespace Ecommerce.Api.Middlewares.ExceptionHandler
{
    /// <summary>
    /// Provides URLs for different RFC types.
    /// </summary>
    public static class RfcTypeUrls
    {
        /// <summary>
        /// URL for the BadRequest RFC.
        /// </summary>
        public const string BadRequest = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

        /// <summary>
        /// URL for the Unauthorized RFC.
        /// </summary>
        public const string Unauthorized = "https://tools.ietf.org/html/rfc9110#section-15.5.2";

        /// <summary>
        /// URL for the NotFound RFC.
        /// </summary>
        public const string NotFound = "https://tools.ietf.org/html/rfc9110#section-15.5.5";

        /// <summary>
        /// URL for the TooManyRequests RFC.
        /// </summary>
        public const string TooManyRequests = "https://tools.ietf.org/html/rfc6585#section-4";

        /// <summary>
        /// URL for the InternalServerError RFC.
        /// </summary>
        public const string InternalServerError = "https://tools.ietf.org/html/rfc9110#section-15.6.1";

        /// <summary>
        /// Default URL for RFC.
        /// </summary>
        public const string Default = "https://tools.ietf.org/html/rfc9110";
    }
}