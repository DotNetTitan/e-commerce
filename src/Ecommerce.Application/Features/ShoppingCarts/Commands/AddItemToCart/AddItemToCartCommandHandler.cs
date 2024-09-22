using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemToCart
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Result<AddItemToCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public AddItemToCartCommandHandler(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<AddItemToCartResponse>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Fail<AddItemToCartResponse>($"Product with ID {request.ProductId} not found.");
            }

            if (!product.CanFulfillOrder(request.Quantity))
            {
                return Result.Fail<AddItemToCartResponse>($"Not enough stock. Available: {product.StockQuantity}, Requested: {request.Quantity}");
            }

            var cart = await _shoppingCartRepository.GetByCustomerIdAsync(request.CustomerId);
            if (cart == null)
            {
                cart = new ShoppingCart { CustomerId = request.CustomerId };
                cart = await _shoppingCartRepository.CreateAsync(cart);
            }

            cart.AddItem(product, request.Quantity);

            await _shoppingCartRepository.UpdateAsync(cart);

            return Result.Ok(new AddItemToCartResponse
            {
                CartId = cart.ShoppingCartId,
                ProductId = request.ProductId,
                Quantity = cart.ShoppingCartItems.First(i => i.ProductId == request.ProductId).Quantity,
                TotalItems = cart.TotalItems,
                TotalPrice = cart.TotalPrice
            });
        }
    }

    public class AddItemToCartCommand : IRequest<Result<AddItemToCartResponse>>
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
        public required int Quantity { get; init; }
    }

    public class AddItemToCartResponse
    {
        public required Guid CartId { get; init; }
        public required Guid ProductId { get; init; }
        public required int Quantity { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}