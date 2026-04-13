namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetAllRestaurantsResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string phoneNumber { get; set; }
        public string ownerName { get; set; }
        public int restaurantCategoryId { get; set; }
        public string? profile { get; set; }
        public List<string>? gallery { get; set; }

    }
}
