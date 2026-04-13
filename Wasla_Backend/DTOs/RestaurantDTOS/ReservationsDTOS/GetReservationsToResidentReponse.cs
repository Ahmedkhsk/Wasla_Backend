namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetReservationsToResidentReponse
    {
        public int id { get; set; }
        public string restaurantId { get; set; }
        public int numberOfPersons { get; set; }
        public string restaurantName { get; set; }
        public string restaurantProfile { get; set; }
        public string restaurantPhone { get; set; }
        public DateOnly reservationDate { get; set; }
        public TimeOnly reservationTime { get; set; }
        public Status status { get; set; }
    }
}
