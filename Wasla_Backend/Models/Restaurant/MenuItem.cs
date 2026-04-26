namespace Wasla_Backend.Models.Restaurant
{
    public class MenuItem
    {
        public int id { get; set; }

        public MultilingualText name { get; set; }

        public decimal price { get; set; }
        public decimal? discountPrice { get; set; }

        public string? imageUrl { get; set; }

        public bool isAvailable { get; set; } = true;

        public int? preparationTime { get; set; }

        [ForeignKey("restaurant")]
        public string restaurantId { get; set; }
        public Restaurant restaurant { get; set; }

        [ForeignKey("category")]
        public int? categoryId { get; set; }
        public MenuItemCategory category { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
