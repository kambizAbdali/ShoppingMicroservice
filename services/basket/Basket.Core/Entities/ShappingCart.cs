namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = [];

        public ShoppingCart() { } // برای AutoMapper/Serialization

        public ShoppingCart(string username, string userId)
        {
            UserName = username;
            UserId = userId;
        }

        public decimal CalculateOroginalPrice()
            => Items.Sum(x => x.Quantity * x.Price);
    }
}
