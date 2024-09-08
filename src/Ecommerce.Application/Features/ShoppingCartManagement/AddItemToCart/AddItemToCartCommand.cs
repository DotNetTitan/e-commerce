using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCartManagement.AddItemToCart
{
    public class AddItemToCartCommand : IRequest<Result<AddItemToCartResponse>>
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
        public required int Quantity { get; init; }
    }
}