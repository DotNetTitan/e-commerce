using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductCommandResponse>>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<CreateProductCommandResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if a product with the same name already exists
            var existingProduct = await _productRepository.GetByNameAsync(request.Name);
            if (existingProduct != null)
            {
                return Result.Fail<CreateProductCommandResponse>($"A product with the name '{request.Name}' already exists.");
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                LowStockThreshold = request.LowStockThreshold
            };

            var createdProduct = await _productRepository.CreateAsync(product);

            if (createdProduct != null)
            {
                return Result.Ok(new CreateProductCommandResponse
                {
                    Id = createdProduct.ProductId,
                    Name = createdProduct.Name,
                    Description = createdProduct.Description,
                    Price = createdProduct.Price,
                    StockQuantity = createdProduct.StockQuantity,
                    CategoryId = createdProduct.CategoryId
                });
            }

            return Result.Fail<CreateProductCommandResponse>("Failed to create product");
        }
    }

    public class CreateProductCommand : IRequest<Result<CreateProductCommandResponse>>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal Price { get; init; }
        public required int StockQuantity { get; init; }
        public required Guid CategoryId { get; init; }
        public required int LowStockThreshold { get; init; }
    }

    public class CreateProductCommandResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
    }
}