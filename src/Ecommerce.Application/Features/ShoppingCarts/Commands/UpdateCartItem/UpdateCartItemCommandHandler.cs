﻿using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, Result<UpdateCartItemResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public UpdateCartItemCommandHandler(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<UpdateCartItemResponse>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _shoppingCartRepository.GetByCustomerIdAsync(request.CustomerId);
            if (cart == null)
            {
                return Result.Fail<UpdateCartItemResponse>($"Shopping cart not found for customer {request.CustomerId}");
            }

            var cartItem = cart.ShoppingCartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (cartItem == null)
            {
                return Result.Fail<UpdateCartItemResponse>($"Product with ID {request.ProductId} not found in the cart");
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Fail<UpdateCartItemResponse>($"Product with ID {request.ProductId} not found");
            }

            if (!product.CanFulfillOrder(request.NewQuantity))
            {
                return Result.Fail<UpdateCartItemResponse>($"Not enough stock. Available: {product.StockQuantity}, Requested: {request.NewQuantity}");
            }

            cartItem.UpdateQuantity(request.NewQuantity);
            cartItem.Price = product.Price;
            await _shoppingCartRepository.UpdateAsync(cart);

            return Result.Ok(new UpdateCartItemResponse
            {
                CartId = cart.ShoppingCartId,
                ProductId = request.ProductId,
                NewQuantity = request.NewQuantity,
                TotalItems = cart.TotalItems,
                TotalPrice = cart.TotalPrice
            });
        }
    }
    public class UpdateCartItemCommand : IRequest<Result<UpdateCartItemResponse>>
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
    }

    public class UpdateCartItemResponse
    {
        public required Guid CartId { get; init; }
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}