using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.OrderManagement.PlaceOrder;
using Ecommerce.Application.Features.OrderManagement.CancelOrder;
using Ecommerce.Application.Features.OrderManagement.GetOrderDetails;
using Ecommerce.Application.Features.OrderManagement.ListUserOrders;
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
        public async Task<ActionResult<PlaceOrderResponse>> PlaceOrder([FromBody] PlaceOrderDto orderDetails)
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

        [HttpGet("{orderId}")]
        public async Task<ActionResult<GetOrderDetailsResponse>> GetOrderDetails(Guid orderId, [FromQuery] Guid customerId)
        {
            var query = new GetOrderDetailsQuery { OrderId = orderId, CustomerId = customerId };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<ListUserOrdersResponse>> ListUserOrders(Guid customerId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListUserOrdersQuery { CustomerId = customerId, PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}