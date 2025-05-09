﻿using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Api.DTOs.Products;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Queries.GetProduct;
using Ecommerce.Application.Features.Products.Queries.ListProducts;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;

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
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto dto)
        {
            var command = new CreateProductCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                LowStockThreshold = dto.LowStockThreshold,
                Thumbnail = dto.Thumbnail,
                Images = dto.Images
            };


            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetProduct), new { productId = result.Value.Id }, result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(Guid productId)
        {
            var query = new GetProductQuery { ProductId = productId };
            
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromForm] UpdateProductDto dto)
        {
            if (productId != dto.ProductId)
            {
                return BadRequest("The Product ID in the URL does not match the Product ID in the request body.");
            }

            var command = new UpdateProductCommand
            {
                ProductId = dto.ProductId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                LowStockThreshold = dto.LowStockThreshold,
                Thumbnail = dto.Thumbnail,
                Images = dto.Images
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            if (result.Errors.Exists(e => e.Message.Contains("not found")))
            {
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Message);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListProducts([FromQuery] ListProductsDto dto)
        {
            var query = new ListProductsQuery
            {
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                SearchTerm = dto.SearchTerm,
                CategoryId = dto.CategoryId
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors.FirstOrDefault()?.Message);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(typeof(DeleteProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var command = new DeleteProductCommand { ProductId = productId };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors.FirstOrDefault()?.Message);
        }
    }
}