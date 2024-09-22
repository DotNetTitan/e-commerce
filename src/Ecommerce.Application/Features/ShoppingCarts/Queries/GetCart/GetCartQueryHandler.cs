using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<GetCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public GetCartQueryHandler(IShoppingCartRepository shoppingCartRepository)
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

    public class GetCartQuery : IRequest<Result<GetCartResponse>>
    {
        public required Guid CustomerId { get; init; }
    }

    public class GetCartResponse
    {
        public required Guid CartId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
        public required List<CartItem> Items { get; init; }
    }

    public class CartItem
    {
        public required Guid ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int Quantity { get; init; }
        public required decimal Price { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}