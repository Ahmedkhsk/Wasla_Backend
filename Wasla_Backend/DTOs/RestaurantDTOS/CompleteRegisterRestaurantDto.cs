namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class CompleteRegisterRestaurantDto : LanDto
    {
        [EmailAddress]
        public string email { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string phoneNumber { get; set; }
        public string ownerName { get; set; }
        public int restaurantCategoryId { get; set; }
        public IFormFile profile { get; set; }
        public List<IFormFile> gallery { get; set; }
    }
}
