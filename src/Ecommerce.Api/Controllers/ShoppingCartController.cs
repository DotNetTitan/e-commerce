using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.ShoppingCartManagement.AddItemToCart;
using Ecommerce.Application.DTOs.ShoppingCartManagement;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/shopping-cart")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShoppingCartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-item")]
        [ProducesResponseType(typeof(AddItemToCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartDto dto)
        {
            var command = new AddItemToCartCommand
            {
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }
    }
}