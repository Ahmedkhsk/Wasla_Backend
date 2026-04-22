namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class OrderItemsResponse
    {
        public int menuItemId { get; set; }
        public int orderItemId { get; set; }
        public string orderItemName { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
