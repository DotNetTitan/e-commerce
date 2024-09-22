using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.DTOs.ShoppingCarts;
using Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemToCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.ClearCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveItemFromCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateCartItem;
using Ecommerce.Application.Features.ShoppingCarts.Queries.GetCart;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers/{customerId}/cart")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShoppingCartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("items/{productId}")]
        [ProducesResponseType(typeof(AddItemToCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItemToCart(Guid customerId, Guid productId, [FromBody] AddItemToCartDto dto)
        {
            if (customerId != dto.CustomerId || productId != dto.ProductId)
            {
                return BadRequest("Customer ID or Product ID in the route does not match the DTO.");
            }

            var command = new AddItemToCartCommand
            {
                CustomerId = customerId,
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

        [HttpGet]
        [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCart(Guid customerId)
        {
            var query = new GetCartQuery
            {
                CustomerId = customerId
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("items/{productId}")]
        [ProducesResponseType(typeof(RemoveItemFromCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItemFromCart(Guid customerId, Guid productId)
        {
            var command = new RemoveItemFromCartCommand
            {
                CustomerId = customerId,
                ProductId = productId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("items/{productId}")]
        [ProducesResponseType(typeof(UpdateCartItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCartItem(Guid customerId, Guid productId, [FromBody] UpdateCartItemDto dto)
        {
            if (customerId != dto.CustomerId || productId != dto.ProductId)
            {
                return BadRequest("Customer ID or Product ID in the route does not match the DTO.");
            }

            var command = new UpdateCartItemCommand
            {
                CustomerId = customerId,
                ProductId = productId,
                NewQuantity = dto.NewQuantity
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ClearCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearCart(Guid customerId)
        {
            var command = new ClearCartCommand
            {
                CustomerId = customerId
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