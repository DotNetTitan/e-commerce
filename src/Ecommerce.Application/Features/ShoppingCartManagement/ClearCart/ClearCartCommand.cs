using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ShoppingCartManagement.ClearCart
{
    public class ClearCartCommand : IRequest<Result<ClearCartResponse>>
    {
        public required Guid CustomerId { get; init; }
    }
}