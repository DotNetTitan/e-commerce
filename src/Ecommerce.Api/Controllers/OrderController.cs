using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.DTOs.Orders;
using Asp.Versioning;
using Ecommerce.Application.Features.Orders.Commands.PlaceOrder;
using Ecommerce.Application.Features.Orders.Commands.CancelOrder;
using Ecommerce.Application.Features.Orders.Queries.GetOrder;
using Ecommerce.Application.Features.Orders.Queries.ListOrders;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlaceOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto orderDetails)
        {
            var command = new PlaceOrderCommand { OrderDetails = orderDetails };
            var result = await _mediator.Send(command);

            if (result.OrderId != Guid.Empty)
            {
                return CreatedAtAction(nameof(GetOrderDetails), new { orderId = result.OrderId }, result);
            }

            return BadRequest("Failed to place the order");
        }

        [HttpPost("{orderId}/cancel")]
        [ProducesResponseType(typeof(CancelOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderDto cancelOrderDto)
        {
            var command = new CancelOrderCommand { OrderId = orderId, CustomerId = cancelOrderDto.CustomerId };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.Errors.First().Message.Contains("not found")
                ? NotFound(result.Errors)
                : BadRequest(result.Errors);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(GetOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderDetails(Guid orderId, [FromQuery] Guid customerId)
        {
            var query = new GetOrderQuery { OrderId = orderId, CustomerId = customerId };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.Errors.First().Message.Contains("not found")
                ? NotFound(result.Errors)
                : BadRequest(result.Errors);
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ListOrdersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListUserOrders(Guid customerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListOrdersQuery { CustomerId = customerId, PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }
    }
}