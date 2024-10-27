using Ecommerce.Application.Common.Models;

namespace Ecommerce.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokensAsync(string userName, string userId);
    }
}