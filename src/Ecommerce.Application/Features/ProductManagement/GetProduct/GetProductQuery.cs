using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ProductManagement.GetProduct
{
    public class GetProductQuery : IRequest<Result<GetProductQueryResponse>>
    {
        public required Guid ProductId { get; init; }
    }
}