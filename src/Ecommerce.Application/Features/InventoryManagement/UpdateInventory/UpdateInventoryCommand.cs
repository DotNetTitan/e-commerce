using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.InventoryManagement.UpdateInventory
{
    public class UpdateInventoryCommand : IRequest<Result<UpdateInventoryResponse>>
    {
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
    }
}