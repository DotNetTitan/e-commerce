using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCartManagement.GetCart
{
    public class GetCartQuery : IRequest<Result<GetCartResponse>>
    {
        public required Guid CustomerId { get; init; }
    }
}