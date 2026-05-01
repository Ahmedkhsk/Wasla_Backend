namespace Wasla_Backend.Models.technician
{
    public class TechnicianBooking : BaseBooking
    {
        public string TechnicianId { get; set; }
        [ForeignKey("TechnicianId")]
        public Technician Technician { get; set; }
        public TechnicianSpecialty Specialty { get; set; }

        public DateTime CreatedAt { get; set; } 
        public TechnicianBookingStatus Status { get; set; }
    }
}
