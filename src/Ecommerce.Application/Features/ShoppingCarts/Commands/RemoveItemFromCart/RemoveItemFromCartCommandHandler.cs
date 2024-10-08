﻿using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, Result<RemoveItemFromCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public RemoveItemFromCartCommandHandler(IShoppingCartRepository shoppingCartRepository)
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

    public class RemoveItemFromCartCommand : IRequest<Result<RemoveItemFromCartResponse>>
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
    }

    public class RemoveItemFromCartResponse
    {
        public required Guid CartId { get; init; }
        public required Guid RemovedProductId { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}