namespace Wasla_Backend.Models.Restaurant
{
    public class Restaurant : ServiceProvider
    {
        public string? gallery { get; set; }
        public int? restaurantCategoryId { get; set; }

        [ForeignKey("restaurantCategoryId")]
        public RestaurantCategory? restaurantCategory { get; set; }

        [NotMapped]
        public List<string>? images
        {
            get => gallery == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(gallery);
            set => gallery = JsonSerializer.Serialize(value);
        }

        public ICollection<MenuItem> items { get; set; }
        public ICollection<MenuItemCategory> categories { get; set; }
    }
}
