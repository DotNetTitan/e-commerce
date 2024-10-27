using Ecommerce.Application.Common.Models;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly RefreshTokenService _refreshTokenService;

        public TokenService(IOptions<AppSettings> appSettings, RefreshTokenService refreshTokenService)
        {
            _appSettings = appSettings.Value;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<TokenResponse> GenerateTokensAsync(string userName, string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName!),
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _appSettings.Jwt.Issuer,
                Audience = _appSettings.Jwt.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}