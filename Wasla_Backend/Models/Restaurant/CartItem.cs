namespace Wasla_Backend.Models.Restaurant
{
    public class CartItem
    {
        public int id { get; set; }

        [ForeignKey("cart")]
        public int cartId { get; set; }
        public Cart cart { get; set; }

        [ForeignKey("menuItem")]
        public int menuItemId { get; set; }
        public MenuItem menuItem { get; set; }

        public int quantity { get; set; }

        public decimal price { get; set; }
    }
}
