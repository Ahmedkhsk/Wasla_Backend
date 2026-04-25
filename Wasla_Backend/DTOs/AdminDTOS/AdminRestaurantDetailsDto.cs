namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminRestaurantDetailsDto(Restaurant restaurant)
    {
        public string businessName { get; set; } = restaurant.BusinessName;
        public string email { get; set; } = restaurant.Email;
        public string description { get; set; } = restaurant.Description;
        public List<string> images { get; set; } = restaurant.images;
    }
}
