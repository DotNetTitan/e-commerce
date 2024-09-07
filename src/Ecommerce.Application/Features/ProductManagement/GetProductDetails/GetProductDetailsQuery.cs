using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ProductManagement.GetProductDetails
{
    public class GetProductDetailsQuery : IRequest<Result<GetProductDetailsResponse>>
    {
        public required Guid ProductId { get; init; }
    }
}