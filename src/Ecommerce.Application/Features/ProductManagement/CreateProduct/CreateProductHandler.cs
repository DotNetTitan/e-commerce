using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.ProductManagement.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            };

            var createdProduct = await _productRepository.CreateAsync(product);

            if (createdProduct != null)
            {
                return Result.Ok(new CreateProductResponse
                {
                    Id = createdProduct.ProductId,
                    Name = createdProduct.Name,
                    Description = createdProduct.Description,
                    Price = createdProduct.Price,
                    StockQuantity = createdProduct.StockQuantity,
                    CategoryId = createdProduct.CategoryId
                });
            }

            return Result.Fail<CreateProductResponse>("Failed to create product");
        }
    }
}