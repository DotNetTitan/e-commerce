using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.CategoryManagement.CreateCategory;
using Ecommerce.Application.Features.CategoryManagement.GetCategory;
using Ecommerce.Application.Features.CategoryManagement.UpdateCategory;
using Ecommerce.Application.Features.CategoryManagement.DeleteCategory;
using Ecommerce.Application.Features.CategoryManagement.ListCategories;
using Ecommerce.Application.DTOs.CategoryManagement;
using System.Net;
using Microsoft.AspNetCore.Authorization;

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
        [ProducesResponseType(typeof(CreateCategoryCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Authorize]
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
        [ProducesResponseType(typeof(GetCategoryQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Authorize]
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
        [ProducesResponseType(typeof(UpdateCategoryCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [Authorize]
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
        [ProducesResponseType(typeof(DeleteCategoryCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Authorize]
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
        [ProducesResponseType(typeof(ListCategoriesQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [Authorize]
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