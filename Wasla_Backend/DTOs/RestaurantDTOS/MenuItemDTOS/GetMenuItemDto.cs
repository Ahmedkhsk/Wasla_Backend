namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetMenuItemDto
    {
        public int id { get; set; }
        public MultilingualText name { get; set; }
        public string nameValue { get; set; }
        public decimal price { get; set; }
        public decimal? discountPrice { get; set; }
        public string? imageUrl { get; set; }
        public int? preparationTime { get; set; }
        public string restaurantId { get; set; }
        public int? categoryId { get; set; }
    }
}
