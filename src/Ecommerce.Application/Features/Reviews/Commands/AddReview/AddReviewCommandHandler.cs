﻿using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Reviews.Commands.AddReview
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Result<AddReviewResponse>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;

        public AddReviewCommandHandler(IReviewRepository reviewRepository, IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<AddReviewResponse>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Fail<AddReviewResponse>($"Product with ID {request.ProductId} not found.");
            }

            var review = new Review
            {
                ProductId = request.ProductId,
                CustomerId = request.CustomerId,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow
            };

            var createdReview = await _reviewRepository.CreateAsync(review);

            if (createdReview != null)
            {
                return Result.Ok(new AddReviewResponse
                {
                    ReviewId = createdReview.ReviewId,
                    ProductId = createdReview.ProductId,
                    CustomerId = createdReview.CustomerId,
                    Rating = createdReview.Rating,
                    Comment = createdReview.Comment,
                    ReviewDate = createdReview.ReviewDate
                });
            }

            return Result.Fail<AddReviewResponse>("Failed to create review");
        }
    }

    public class AddReviewCommand : IRequest<Result<AddReviewResponse>>
    {
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
    }

    public class AddReviewResponse
    {
        public required Guid ReviewId { get; init; }
        public required Guid ProductId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public required DateTime ReviewDate { get; init; }
    }
}