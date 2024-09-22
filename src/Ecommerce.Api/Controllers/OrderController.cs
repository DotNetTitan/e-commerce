﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.OrderManagement.PlaceOrder;
using Ecommerce.Application.Features.OrderManagement.CancelOrder;
using Ecommerce.Application.Features.OrderManagement.GetOrderDetails;
using Ecommerce.Application.Features.OrderManagement.ListUserOrders;
using Ecommerce.Application.DTOs.OrderManagement;
using Asp.Versioning;

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
        [ProducesResponseType(typeof(CancelOrderCommandResponse), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(GetOrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderDetails(Guid orderId, [FromQuery] Guid customerId)
        {
            var query = new GetOrderDetailsQuery { OrderId = orderId, CustomerId = customerId };
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
        [ProducesResponseType(typeof(ListUserOrdersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListUserOrders(Guid customerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListUserOrdersQuery { CustomerId = customerId, PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }
    }
}