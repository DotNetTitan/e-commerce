using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<ClearCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ClearCartCommandHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Result<ClearCartResponse>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var success = await _shoppingCartRepository.ClearAsync(request.CustomerId);
            if (!success)
            {
                return Result.Fail<ClearCartResponse>($"Failed to clear shopping cart for customer {request.CustomerId}");
            }

            return Result.Ok(new ClearCartResponse
            {
                CustomerId = request.CustomerId,
                Success = true
            });
        }
    }

    public class ClearCartCommand : IRequest<Result<ClearCartResponse>>
    {
        public required Guid CustomerId { get; init; }
    }

    public class ClearCartResponse
    {
        public required Guid CustomerId { get; init; }
        public required bool Success { get; init; }
    }
}