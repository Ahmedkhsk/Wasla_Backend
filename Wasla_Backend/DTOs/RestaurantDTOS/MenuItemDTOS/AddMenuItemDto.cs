namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class AddMenuItemDto
    {
        public MultilingualText name { get; set; }
        public decimal price { get; set; }
        public decimal? discountPrice { get; set; }
        public IFormFile imageUrl { get; set; }
        public int? preparationTime { get; set; }
        public string restaurantId { get; set; }
        public int? categoryId { get; set; }
    }
}
