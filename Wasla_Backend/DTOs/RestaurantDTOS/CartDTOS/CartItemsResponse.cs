namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class CartItemsResponse
    {
        public int cartItemId { get; set; }
        public int menuItemId { get; set; }
        public string menuItemName { get; set; }
        public string menuItemCategoryName { get; set; }
        public string imageUrl { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
    }
}
