﻿namespace Ecommerce.Infrastructure.Settings
{
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the URL for resending confirmation email.
        /// </summary>
        public string ResendConfirmationEmailUrl { get; init; } = default!;

        /// <summary>
        /// Gets or sets the URL for password reset.
        /// </summary>
        public string PasswordResetUrl { get; init; } = default!;

        /// <summary>
        /// Gets or sets the URL for confirmation email.
        /// </summary>
        public string ConfirmationEmailUrl { get; init; } = default!;

        /// <summary>
        /// Gets or sets the email sender address.
        /// </summary>
        public string EmailSenderAddress { get; init; } = default!;
        
        /// <summary>
        /// Gets or sets the Azure BlobStorage ContainerName.
        /// </summary>
        public string AzureBlobStorageContainerName { get; init; } = default!;

        /// <summary>
        /// Gets or sets the JWT settings.
        /// </summary>
        public JwtSettings Jwt { get; init; } = default!;
    }

    /// <summary>
    /// Represents the JWT settings.
    /// </summary>
    public record JwtSettings
    {
        /// <summary>
        /// Gets or sets the JWT key.
        /// </summary>
        public string Key { get; init; } = default!;

        /// <summary>
        /// Gets or sets the JWT issuer.
        /// </summary>
        public string Issuer { get; init; } = default!;

        /// <summary>
        /// Gets or sets the JWT audience.
        /// </summary>
        public string Audience { get; init; } = default!;
    }
}