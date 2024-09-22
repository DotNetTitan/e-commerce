using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<ClearCartCommandResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ClearCartCommandHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<Result<ClearCartCommandResponse>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var success = await _shoppingCartRepository.ClearAsync(request.CustomerId);
            if (!success)
            {
                return Result.Fail<ClearCartCommandResponse>($"Failed to clear shopping cart for customer {request.CustomerId}");
            }

            return Result.Ok(new ClearCartCommandResponse
            {
                CustomerId = request.CustomerId,
                Success = true
            });
        }
    }

    public class ClearCartCommand : IRequest<Result<ClearCartCommandResponse>>
    {
        public required Guid CustomerId { get; init; }
    }

    public class ClearCartCommandResponse
    {
        public required Guid CustomerId { get; init; }
        public required bool Success { get; init; }
    }
}