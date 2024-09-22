using Ecommerce.Application.Features.Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.Authentication.Register;
using Ecommerce.Application.DTOs.Authentication;
using Asp.Versioning;
using Ecommerce.Application.Features.Authentication.ConfirmEmail;
using Ecommerce.Application.Features.Authentication.ForgotPassword;
using Ecommerce.Application.Features.Authentication.ResetPassword;
using Ecommerce.Application.Features.Authentication.ResendEmailConfirmation;

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
        [ProducesResponseType(typeof(RegisterCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var command = new RegisterCommand
            {
                UserName = register.UserName,
                Email = register.Email,
                Password = register.Password
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value.Message) : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var command = new LoginQuery
            {
                UserName = login.UserName,
                Password = login.Password
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Errors);
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(typeof(ConfirmEmailCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
        {
            var command = new ConfirmEmailCommand
            {
                Email = email,
                Token = token
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value.Message) : BadRequest(result.Errors);
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(RequestPasswordResetCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var command = new RequestPasswordResetCommand
            {
                Email = forgotPassword.Email
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value.Message) : BadRequest(result.Errors);
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ResetPasswordCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            var command = new ResetPasswordCommand
            {
                Email = resetPassword.Email,
                Token = resetPassword.Token,
                NewPassword = resetPassword.NewPassword
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value.Message) : BadRequest(result.Errors);
        }

        [HttpPost("resend-email-confirmation")]
        [ProducesResponseType(typeof(ResendEmailConfirmationCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationDto resendEmailConfirmation)
        {
            var command = new ResendEmailConfirmationCommand
            {
                Email = resendEmailConfirmation.Email
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result.Value.Message) : BadRequest(result.Errors);
        }
    }
}