namespace Ecommerce.Application.Features.InventoryManagement.UpdateInventory
{
    public class UpdateInventoryResponse
    {
        public required Guid ProductId { get; init; }
        public required int NewStockQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
    }
}