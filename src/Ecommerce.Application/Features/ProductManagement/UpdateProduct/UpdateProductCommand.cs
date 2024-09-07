using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ProductManagement.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result<UpdateProductResponse>>
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal Price { get; init; }
        public required int StockQuantity { get; init; }
        public required Guid CategoryId { get; init; }
    }
}