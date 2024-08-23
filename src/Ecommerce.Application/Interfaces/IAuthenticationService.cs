using Ecommerce.Application.DTOs.Authentication;
using FluentResults;

namespace Ecommerce.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<string>> RegisterUserAsync(string userName, string email, string password);
        Task<Result> LoginUserAsync(string userName, string password);
        Task<UserDto?> GetUserByUsernameAsync(string userName);
        Task<Result> ConfirmEmailAsync(string email, string token);
    }
}