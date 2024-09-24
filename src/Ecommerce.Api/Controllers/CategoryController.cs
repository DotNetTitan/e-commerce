using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Api.DTOs.Categories;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Features.Categories.Commands.CreateCategory;
using Ecommerce.Application.Features.Categories.Commands.DeleteCategory;
using Ecommerce.Application.Features.Categories.Commands.UpdateCategory;
using Ecommerce.Application.Features.Categories.Queries.GetCategory;
using Ecommerce.Application.Features.Categories.Queries.ListCategories;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateCategoryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var command = new CreateCategoryCommand
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCategory), new { id = result.Value.CategoryId }, result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(GetCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            var query = new GetCategoryQuery { CategoryId = categoryId };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(UpdateCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] UpdateCategoryDto dto)
        {
            if (categoryId != dto.CategoryId)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            var command = new UpdateCategoryCommand
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors.FirstOrDefault()?.Message);
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(typeof(DeleteCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var command = new DeleteCategoryCommand { CategoryId = categoryId };
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            if (result.Errors.Exists(e => e.Message.Contains("not found")))
            {
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the product");
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListCategoriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListCategories([FromQuery] ListCategoriesDto dto)
        {
            var query = new ListCategoriesQuery
            {
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                SearchTerm = dto.SearchTerm
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors.FirstOrDefault()?.Message);
        }
    }
}