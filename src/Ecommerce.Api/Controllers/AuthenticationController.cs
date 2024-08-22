using Ecommerce.Application.Features.Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.Authentication.Register;
using Ecommerce.Application.DTOs.Authentication;
using Asp.Versioning;

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
                Username = register.Username,
                Email = register.Email,
                Password = register.Password
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var command = new LoginCommand
            {
                Username = login.Username,
                Password = login.Password
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Unauthorized(result.Errors);
        }
    }
}