using NSubstitute;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Features.Reviews.Commands.AddReview;
using Ecommerce.Application.Features.Reviews.Commands.UpdateReview;
using Ecommerce.Application.Features.Reviews.Commands.DeleteReview;
using Ecommerce.Application.Features.Reviews.Queries.GetReview;
using Ecommerce.Application.Features.Reviews.Queries.ListReviews;

namespace Ecommerce.Application.Tests
{
    public class ReviewUnitTests
    {
        private readonly IReviewRepository _reviewRepository = Substitute.For<IReviewRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

        [Fact]
        public async Task AddReview_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var command = new AddReviewCommand
            {
                ProductId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great product!"
            };

            var product = new Product
            {
                ProductId = command.ProductId, 
                Description = "Test Description!", 
                Name = "Test Name!",
                CategoryId = Guid.NewGuid(), 
                Price = 10
            };
            _productRepository.GetByIdAsync(command.ProductId).Returns(product);

            var createdReview = new Review
            {
                ReviewId = Guid.NewGuid(),
                ProductId = command.ProductId,
                CustomerId = command.CustomerId,
                Rating = command.Rating,
                Comment = command.Comment,
                ReviewDate = DateTime.UtcNow
            };

            _reviewRepository.CreateAsync(Arg.Any<Review>()).Returns(createdReview);

            var handler = new AddReviewCommandHandler(_reviewRepository, _productRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.ProductId, result.Value.ProductId);
            Assert.Equal(command.CustomerId, result.Value.CustomerId);
            Assert.Equal(command.Rating, result.Value.Rating);
            Assert.Equal(command.Comment, result.Value.Comment);
        }

        [Fact]
        public async Task UpdateReview_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var command = new UpdateReviewCommand
            {
                ReviewId = reviewId,
                ProductId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Updated comment"
            };

            var existingReview = new Review
            {
                ReviewId = reviewId,
                ProductId = command.ProductId,
                CustomerId = command.CustomerId,
                Rating = 3,
                Comment = "Original comment",
                ReviewDate = DateTime.UtcNow.AddDays(-1)
            };

            _reviewRepository.GetByIdAsync(reviewId).Returns(existingReview);
            _reviewRepository.UpdateAsync(Arg.Any<Review>()).Returns(x => x.Arg<Review>());

            var handler = new UpdateReviewCommandHandler(_reviewRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.ReviewId, result.Value.ReviewId);
            Assert.Equal(command.Rating, result.Value.Rating);
            Assert.Equal(command.Comment, result.Value.Comment);
        }

        [Fact]
        public async Task DeleteReview_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var command = new DeleteReviewCommand
            {
                ReviewId = reviewId,
                ProductId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid()
            };

            var existingReview = new Review
            {
                ReviewId = reviewId,
                ProductId = command.ProductId,
                CustomerId = command.CustomerId,
                Rating = 3,
                Comment = "Comment to delete",
                ReviewDate = DateTime.UtcNow.AddDays(-1)
            };

            _reviewRepository.GetByIdAsync(reviewId).Returns(existingReview);
            _reviewRepository.DeleteAsync(Arg.Any<Review>()).Returns(true);

            var handler = new DeleteReviewCommandHandler(_reviewRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.ReviewId, result.Value.ReviewId);
            Assert.True(result.Value.IsDeleted);
        }

        [Fact]
        public async Task GetReview_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var query = new GetReviewQuery
            {
                ReviewId = reviewId,
                ProductId = productId
            };

            var review = new Review
            {
                ReviewId = reviewId,
                ProductId = productId,
                CustomerId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Test comment",
                ReviewDate = DateTime.UtcNow.AddDays(-1)
            };

            _reviewRepository.GetByIdAsync(reviewId).Returns(review);

            var handler = new GetReviewQueryHandler(_reviewRepository);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(review.ReviewId, result.Value.ReviewId);
            Assert.Equal(review.ProductId, result.Value.ProductId);
            Assert.Equal(review.CustomerId, result.Value.CustomerId);
            Assert.Equal(review.Rating, result.Value.Rating);
            Assert.Equal(review.Comment, result.Value.Comment);
            Assert.Equal(review.ReviewDate, result.Value.ReviewDate);
        }

        [Fact]
        public async Task ListReviews_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var query = new ListReviewsQuery
            {
                ProductId = productId,
                PageNumber = 1,
                PageSize = 10
            };

            var reviews = new List<Review>
            {
                new Review
                {
                    ReviewId = Guid.NewGuid(), ProductId = productId, CustomerId = Guid.NewGuid(), Rating = 4,
                    Comment = "Review 1", ReviewDate = DateTime.UtcNow.AddDays(-1)
                },
                new Review
                {
                    ReviewId = Guid.NewGuid(), ProductId = productId, CustomerId = Guid.NewGuid(), Rating = 5,
                    Comment = "Review 2", ReviewDate = DateTime.UtcNow.AddDays(-2)
                }
            };

            _reviewRepository.GetReviewsByProductIdAsync(productId, query.PageNumber, query.PageSize)
                .Returns((reviews, reviews.Count));

            var handler = new ListReviewsQueryHandler(_reviewRepository);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.ProductId);
            Assert.Equal(reviews.Count, result.Value.Reviews.Count);
            Assert.Equal(reviews.Count, result.Value.TotalCount);
            Assert.Equal(query.PageNumber, result.Value.PageNumber);
            Assert.Equal(query.PageSize, result.Value.PageSize);
        }
    }
}