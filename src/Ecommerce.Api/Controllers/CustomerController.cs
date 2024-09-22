using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using Ecommerce.Application.Features.Customers.Commands.EditCustomer;
using Ecommerce.Application.Features.Customers.Queries.ViewCustomer;
using Ecommerce.Application.DTOs.Customers;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(ViewCustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var query = new ViewCustomerQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);

            if (result.IsFailed)
            {
                return result.Errors.First().Message == "Customer not found."
                    ? NotFound(result.Errors)
                    : BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("{customerId}")]
        [ProducesResponseType(typeof(EditCustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditCustomer(Guid customerId, [FromBody] EditCustomerDto editCustomerDto)
        {
            if (customerId != editCustomerDto.CustomerId)
            {
                return BadRequest("The customerId in the URL does not match the one in the request body.");
            }

            var command = new EditCustomerCommand
            {
                CustomerId = editCustomerDto.CustomerId,
                FirstName = editCustomerDto.FirstName,
                LastName = editCustomerDto.LastName,
                Address = editCustomerDto.Address
            };

            var result = await _mediator.Send(command);

            if (result.IsFailed)
            {
                return result.Errors.First().Message == "Customer not found."
                    ? NotFound(result.Errors)
                    : BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}