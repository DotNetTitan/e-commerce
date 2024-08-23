using Ecommerce.Application.DTOs.Authentication;
using FluentResults;

namespace Ecommerce.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<string>> RegisterUserAsync(string username, string email, string password);
        Task<Result> LoginUserAsync(string username, string password);
        Task<UserDto?> GetUserByUsernameAsync(string userName);
        Task<Result> ConfirmEmailAsync(string email, string token);
    }
}