namespace Ecommerce.Domain.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {
            ShoppingCartId = Guid.NewGuid();
            ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public Guid ShoppingCartId { get; set; }
        public required Guid CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}