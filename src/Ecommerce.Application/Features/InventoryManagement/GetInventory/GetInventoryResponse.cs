namespace Ecommerce.Application.Features.InventoryManagement.GetInventory
{
    public class GetInventoryResponse
    {
        public required List<InventoryItem> InventoryItems { get; init; }
    }

    public class InventoryItem
    {
        public required Guid ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int StockQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
    }
}