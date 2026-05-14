namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetRestaurantResponse
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public int numberOfCompletedOrders { get; set; }
        public string description { get; set; }
        public string phoneNumber { get; set; }
        public string ownerName { get; set; }
        public int restaurantCategoryId { get; set; }
        public string restaurantCategoryName { get; set; }
        public string? profile { get; set; }
        public List<string>? gallery { get; set; }
        public bool isAvailable { get; set; }
    }
}
