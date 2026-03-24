namespace Wasla_Backend.DTOs.TechnicianDtos
{
    public class BookingDetailsForTechnicianDto
    {
        public int BookingId { get; set; }
        public string ResidentName { get; set; }
        public string ResidentPhone { get; set; }
        public string ResidentImage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double price { get; set; }
        public DateTime BookingDate { get; set; }
        public TechnicianBookingStatus Status { get; set; }
    }
}
