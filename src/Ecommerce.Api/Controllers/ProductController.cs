using Asp.Versioning;
using Ecommerce.Application.Features.ProductManagement.CreateProduct;
using Ecommerce.Application.Features.ProductManagement.GetProductDetails;
using Ecommerce.Application.Features.ProductManagement.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.ProductManagement.ListProducts;
using Ecommerce.Application.Features.ProductManagement.DeleteProduct;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProduct), new { id = result.Value.Id }, result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetProductDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var query = new GetProductDetailsQuery { ProductId = id };
            
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            if (result.Errors.Any(e => e.Message.Contains("not found")))
            {
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Message);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListProducts([FromQuery] ListProductsQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return StatusCode(500, "An error occurred while retrieving the products");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var command = new DeleteProductCommand { ProductId = id };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            if (result.Errors.Any(e => e.Message.Contains("not found")))
            {
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            return StatusCode(500, "An error occurred while deleting the product");
        }
    }
}