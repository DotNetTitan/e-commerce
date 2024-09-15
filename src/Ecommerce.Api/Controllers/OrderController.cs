using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.OrderManagement.PlaceOrder;
using Ecommerce.Application.Features.OrderManagement.CancelOrder;
using Ecommerce.Application.DTOs.OrderManagement;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("place")]
        public async Task<ActionResult<int>> PlaceOrder([FromBody] PlaceOrderDto orderDetails)
        {
            var command = new PlaceOrderCommand { OrderDetails = orderDetails };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("cancel")]
        public async Task<ActionResult<bool>> CancelOrder([FromBody] CancelOrderDto cancelOrderDto)
        {
            var command = new CancelOrderCommand { OrderId = cancelOrderDto.OrderId, CustomerId = cancelOrderDto.CustomerId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}