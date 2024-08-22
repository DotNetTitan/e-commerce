using Ecommerce.Application.Common.Models;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Tokens
{
    /// <summary>
    /// Service for generating, validating, and invalidating refresh tokens.
    /// </summary>
    public class RefreshTokenService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenService"/> class.
        /// </summary>
        /// <param name="context">The identity database context.</param>
        public RefreshTokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generate and store a new refresh token.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The generated refresh token.</returns>
        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                IdentityId = userId,
                ExpirationDate = DateTime.UtcNow.AddDays(30)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        /// <summary>
        /// Validate an existing refresh token.
        /// </summary>
        /// <param name="token">The refresh token to validate.</param>
        /// <returns>True if the refresh token is valid, otherwise false.</returns>
        public async Task<bool> ValidateRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null || refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Invalidate a refresh token.
        /// </summary>
        /// <param name="token">The refresh token to invalidate.</param>
        public async Task InvalidateRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
            }
        }
    }
}