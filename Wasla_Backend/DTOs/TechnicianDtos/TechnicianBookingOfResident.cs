namespace Wasla_Backend.DTOs.TechnicianDtos
{
    public class TechnicianBookingOfResident
    {
        public int BookingId { get; set; }
        public string TechnicianName { get; set; }
        public string TechnicianPhone { get; set; }
        public string TechnicianImage { get; set; }
        public double price { get; set; }
        public DateTime BookingDate { get; set; }
        public TechnicianSpecialty TechnicianSpeciality { get; set; }
        public TechnicianBookingStatus Status { get; set; }
    }
}
