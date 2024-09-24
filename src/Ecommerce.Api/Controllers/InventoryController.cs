using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Features.Inventory.Queries.GetInventory;
using Ecommerce.Application.Features.Inventory.Commands.UpdateInventory;
using Ecommerce.Api.DTOs.Inventory;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products/{productId}/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetInventoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInventory([FromQuery] GetInventoryDto getInventoryDto)
        {
            var query = new GetInventoryQuery
            {
                PageNumber = getInventoryDto.PageNumber,
                PageSize = getInventoryDto.PageSize,
                SearchTerm = getInventoryDto.SearchTerm,
                LowStockOnly = getInventoryDto.LowStockOnly,
                CategoryId = getInventoryDto.CategoryId
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UpdateInventoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInventory(Guid productId, [FromBody] UpdateInventoryDto updateInventoryDto)
        {
            if (productId != updateInventoryDto.ProductId)
            {
                return BadRequest("Product ID in the route does not match the command.");
            }

            var command = new UpdateInventoryCommand
            {
                ProductId = updateInventoryDto.ProductId,
                NewQuantity = updateInventoryDto.NewQuantity,
                LowStockThreshold = updateInventoryDto.LowStockThreshold
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors);
        }
    }
}