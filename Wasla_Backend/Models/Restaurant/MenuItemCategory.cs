namespace Wasla_Backend.Models.Restaurant
{
    public class MenuItemCategory
    {
        public int id { get; set; }

        public MultilingualText name { get; set; }

        [ForeignKey("restaurant")] 
        public string restaurantId { get; set; }

        public Restaurant restaurant { get; set; }

        public ICollection<MenuItem> items { get; set; }
    }
}
