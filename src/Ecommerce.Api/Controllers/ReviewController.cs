using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Ecommerce.Application.DTOs.Reviews;
using Ecommerce.Application.Features.Reviews.Commands.DeleteReview;
using Ecommerce.Application.Features.Reviews.Commands.AddReview;
using Ecommerce.Application.Features.Reviews.Commands.UpdateReview;
using Ecommerce.Application.Features.Reviews.Queries.GetReview;
using Ecommerce.Application.Features.Reviews.Queries.ListReviews;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products/{productId}/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(AddReviewResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddReview(Guid productId, [FromBody] AddReviewDto dto)
        {
            var command = new AddReviewCommand
            {
                ProductId = productId,
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetReview), new { productId, reviewId = result.Value.ReviewId }, result.Value);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListReviewsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListReviews(Guid productId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListReviewsQuery { ProductId = productId, PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(typeof(GetReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReview(Guid productId, Guid reviewId)
        {
            var query = new GetReviewQuery { ProductId = productId, ReviewId = reviewId };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Errors);
        }

        [HttpPut("{reviewId}")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReview(Guid productId, Guid reviewId, [FromBody] UpdateReviewDto dto)
        {
            var command = new UpdateReviewCommand
            {
                ReviewId = reviewId,
                ProductId = productId,
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.Errors.First().Message.Contains("not found")
                ? NotFound(result.Errors)
                : BadRequest(result.Errors);
        }

        [HttpDelete("{reviewId}")]
        [Authorize]
        [ProducesResponseType(typeof(DeleteReviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(Guid productId, Guid reviewId, [FromQuery] Guid customerId)
        {
            var command = new DeleteReviewCommand
            {
                ReviewId = reviewId,
                ProductId = productId,
                CustomerId = customerId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.Errors.First().Message.Contains("not found")
                ? NotFound(result.Errors)
                : BadRequest(result.Errors);
        }
    }
}