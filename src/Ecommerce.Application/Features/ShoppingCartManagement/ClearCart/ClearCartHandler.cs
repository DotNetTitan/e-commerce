using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ShoppingCartManagement.ClearCart
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand, Result<ClearCartResponse>>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ClearCartHandler(IShoppingCartRepository shoppingCartRepository)
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
}