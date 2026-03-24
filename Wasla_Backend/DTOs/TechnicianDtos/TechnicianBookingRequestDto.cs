namespace Wasla_Backend.DTOs.TechnicianDtos
{
    public class TechnicianBookingRequestDto
    {
        public string ResidentId { get; set; }
        public string TechnicianId { get; set; }
        public double Price { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
