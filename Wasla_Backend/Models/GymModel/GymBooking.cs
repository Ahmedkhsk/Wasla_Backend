namespace Wasla_Backend.Models.GymModel
{
    public class GymBooking:BaseBooking
    {
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Package Service { get; set; }
        public decimal price { get; set; }
        public string GymId { get; set; }
        public bool IsQrUsed { get; set; } = false;
        public DateTime QrUsedAt { get; set; }

        [ForeignKey("GymId")]
        public Gym Gym { get; set; }
        public GymBookingStatus BookingStatus { get; set; } = GymBookingStatus.Active;
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public GymServiceType GymServiceType { get; set; }


    }
}
