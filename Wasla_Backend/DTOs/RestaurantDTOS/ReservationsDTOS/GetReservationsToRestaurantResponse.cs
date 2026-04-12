namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetReservationsToRestaurantResponse
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string profile { get; set; }
        public string phone { get; set; }
        public string restaurantId { get; set; }
        public int numberOfPersons { get; set; }
        public DateOnly reservationDate { get; set; }
        public TimeOnly reservationTime { get; set; }
        public Status status { get; set; }
    }
}
