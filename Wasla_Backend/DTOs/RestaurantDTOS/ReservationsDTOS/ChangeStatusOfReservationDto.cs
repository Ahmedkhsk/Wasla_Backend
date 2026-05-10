namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class ChangeStatusOfReservationDto : LanDto
    {
        public bool isResident { get; set; }
        public int reservationId { get; set; }
        public Status status { get; set; }
        
    }
}
