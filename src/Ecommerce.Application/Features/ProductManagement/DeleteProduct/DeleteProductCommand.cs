using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ProductManagement.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result<DeleteProductResponse>>
    {
        public required Guid ProductId { get; init; }
    }
}