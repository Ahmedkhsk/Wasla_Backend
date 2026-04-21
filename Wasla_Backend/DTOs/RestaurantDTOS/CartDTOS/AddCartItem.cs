namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class AddCartItem
    {
        public int menuItemId { get; set; }
        public string residentId { get; set; }
        public string restaurantId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
