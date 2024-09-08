using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCartManagement.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, Result<GetCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public GetCartHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Result<GetCartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _shoppingCartRepository.GetByCustomerIdAsync(request.CustomerId);

            if (cart == null)
            {
                return Result.Fail<GetCartResponse>($"Shopping cart not found for customer {request.CustomerId}");
            }

            var response = new GetCartResponse
            {
                CartId = cart.ShoppingCartId,
                CustomerId = cart.CustomerId,
                TotalItems = cart.TotalItems,
                TotalPrice = cart.TotalPrice,
                Items = cart.ShoppingCartItems.Select(item => new CartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Unknown",
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };

            return Result.Ok(response);
        }
    }
}