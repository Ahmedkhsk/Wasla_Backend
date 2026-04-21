namespace Wasla_Backend.Models.Restaurant
{
    public class OrderItem
    {
        public int id { get; set; }

        public int orderId { get; set; }

        [ForeignKey("orderId")]
        public Order order { get; set; }

        public int menuItemId { get; set; }

        [ForeignKey("menuItemId")]
        public MenuItem menuItem { get; set; }
        public int quantity { get; set; }

        public decimal price { get; set; }
    }
}
