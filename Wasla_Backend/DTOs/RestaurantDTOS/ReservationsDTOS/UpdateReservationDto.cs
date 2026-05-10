namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class UpdateReservationDto
    {
        public int reservationId { get; set; }
        public int numberOfPersons { get; set; }
        public DateOnly reservationDate { get; set; }
        public TimeOnly reservationTime { get; set; }
    }
}
