namespace Wasla_Backend.Models.Restaurant
{
    public class Cart
    {
        public int id { get; set; }

        [ForeignKey("resident")]
        public string residentId { get; set; }
        public Resident resident { get; set; }

        [ForeignKey("restaurant")]
        public string restaurantId { get; set; }
        public Restaurant restaurant { get; set; }

        public ICollection<CartItem> items { get; set; }
    }
}
