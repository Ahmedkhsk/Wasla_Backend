namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class CartItemsResponse
    {
        public int cartItemId { get; set; }
        public int menuItemId { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
    }
}
