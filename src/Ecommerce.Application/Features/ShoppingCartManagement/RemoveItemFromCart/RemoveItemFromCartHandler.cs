using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCartManagement.RemoveItemFromCart
{
    public class RemoveItemFromCartHandler : IRequestHandler<RemoveItemFromCartCommand, Result<RemoveItemFromCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public RemoveItemFromCartHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Result<RemoveItemFromCartResponse>> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _shoppingCartRepository.GetByCustomerIdAsync(request.CustomerId);
            if (cart == null)
            {
                return Result.Fail<RemoveItemFromCartResponse>($"Shopping cart not found for customer {request.CustomerId}");
            }

            cart.RemoveItem(request.ProductId);

            await _shoppingCartRepository.UpdateAsync(cart);

            return Result.Ok(new RemoveItemFromCartResponse
            {
                CartId = cart.ShoppingCartId,
                RemovedProductId = request.ProductId,
                TotalItems = cart.TotalItems,
                TotalPrice = cart.TotalPrice
            });
        }
    }
}