namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class UpdateMenuItemDto
    {
        public int id { get; set; }
        public MultilingualText name { get; set; }
        public decimal price { get; set; }
        public decimal? discountPrice { get; set; }
        public IFormFile? imageUrl { get; set; }
        public int? preparationTime { get; set; }
    }
}
