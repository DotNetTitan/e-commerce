﻿using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result> RegisterUserAsync(string username, string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Fail(result.Errors.Select(e => e.Description));
        }

        public async Task<Result> LoginUserAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Fail(["Invalid login attempt."]);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
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
    }
}