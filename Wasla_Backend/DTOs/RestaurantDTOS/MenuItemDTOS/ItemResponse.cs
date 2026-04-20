namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class ItemResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal? discountPrice { get; set; }
        public string imageUrl { get; set; }
        public int? preparationTime { get; set; }
        public bool isAvailable { get; set; }
    }
}
