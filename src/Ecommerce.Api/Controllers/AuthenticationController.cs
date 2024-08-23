using Ecommerce.Application.Features.Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.Authentication.Register;
using Ecommerce.Application.DTOs.Authentication;
using Asp.Versioning;
using Ecommerce.Application.Features.Authentication.ConfirmEmail;
using Ecommerce.Application.Common.Models;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var command = new RegisterCommand
            {
                UserName = register.UserName,
                Email = register.Email,
                Password = register.Password
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value.Message);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var command = new LoginCommand
            {
                UserName = login.UserName,
                Password = login.Password
            };

            var result = await _mediator.Send(command);

            var tokenResponse = new TokenResponse
            {
                AccessToken = result.Value.AccessToken,
                RefreshToken = result.Value.RefreshToken
            };

            if (result.IsSuccess)
            {
                return Ok(tokenResponse);
            }

            return Unauthorized(result.Errors);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            var command = new ConfirmEmailCommand
            {
                Email = email,
                Token = token
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value.Message);
            }

            return BadRequest(result.Errors);
        }
    }
}