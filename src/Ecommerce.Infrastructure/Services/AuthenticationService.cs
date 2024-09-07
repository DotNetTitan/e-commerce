using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Ecommerce.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICustomerRepository _customerRepository;

        public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ICustomerRepository customerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerRepository = customerRepository;
        }

        public async Task<Result<string>> RegisterUserAsync(string userName, string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var customer = new Customer
                {
                    IdentityId = user.Id,
                };

                await _customerRepository.AddAsync(customer);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                return Result.Ok(encodedToken);
            }

            return Result.Fail(result.Errors.Select(e => e.Description));
        }

        public async Task<Result> LoginUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Result.Fail(["User not found."]);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Result.Fail(["Email is not confirmed."]);
            }

            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (result.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Fail(["Invalid login attempt. Please check your credentials and try again."]);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!
            };
        }

        public async Task<Result> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Fail(result.Errors.Select(e => e.Description));
        }

        public async Task<Result<string>> GetPasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return Result.Ok(encodedToken);
        }

        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (result.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Fail(result.Errors.Select(e => e.Description));
        }

        public async Task<Result> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName) ?? throw new UserNotFoundException();

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Fail(result.Errors.Select(e => e.Description));
        }

        public async Task<Result<string>> GetEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return Result.Ok(encodedToken);
        }
    }
}