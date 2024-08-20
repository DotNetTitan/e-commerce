namespace Ecommerce.Domain.Entities
{
   public class ShoppingCartItem : BaseEntity
    {
        public ShoppingCartItem()
        {
            ShoppingCartItemId = Guid.NewGuid();
        }

        public Guid ShoppingCartItemId { get; set; }
        public required Guid ShoppingCartId { get; set; }
        public required ShoppingCart ShoppingCart { get; set; }
        public required Guid ProductId { get; set; }
        public required Product Product { get; set; }
        public required int Quantity { get; set; }
    }
}