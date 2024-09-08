using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCartManagement.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<Result<UpdateCartItemResponse>>
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
    }
}
