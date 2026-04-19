namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class AddReservationDto
    {
        public string userId { get; set; }
        public string restaurantId { get; set; }
        public int numberOfPersons { get; set; }
        public DateOnly reservationDate { get; set; }
        public TimeOnly reservationTime { get; set; }
    }
}
