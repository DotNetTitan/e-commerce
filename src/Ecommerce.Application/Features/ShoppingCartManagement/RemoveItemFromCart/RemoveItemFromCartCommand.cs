using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCartManagement.RemoveItemFromCart
{
    public class RemoveItemFromCartCommand : IRequest<Result<RemoveItemFromCartResponse>>
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
    }
}
