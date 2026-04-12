namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class ChangeStatusOfReservationDto : LanDto
    {
        public int reservationId { get; set; }
        public Status status { get; set; }
        
    }
}
