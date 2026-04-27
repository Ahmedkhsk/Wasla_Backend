namespace Wasla_Backend.Models.Restaurant
{
    public class Reservations
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string restaurantId { get; set; }
        public int numberOfPersons { get; set; }
        public DateOnly reservationDate { get; set; }
        public TimeOnly reservationTime { get; set; }
        public Status status { get; set; }

        [ForeignKey("userId")]
        public Resident user { get; set; }

        [ForeignKey("restaurantId")]
        public Restaurant restaurants { get; set; }

        public string? QRCode { get; set; }

    }
}
