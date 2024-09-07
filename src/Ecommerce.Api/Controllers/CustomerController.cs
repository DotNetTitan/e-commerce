using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ecommerce.Application.Features.CustomerManagement.EditCustomer;
using Ecommerce.Application.Features.CustomerManagement.ViewCustomerDetails;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    [Authorize] // Ensure only authenticated users can access these endpoints
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(ViewCustomerDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCustomerDetails(Guid customerId)
        {
            var query = new ViewCustomerDetailsQuery { CustomerId = customerId };
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
        public async Task<IActionResult> EditCustomer(Guid customerId, [FromBody] EditCustomerCommand command)
        {
            if (customerId != command.CustomerId)
            {
                return BadRequest("The customerId in the URL does not match the one in the request body.");
            }

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