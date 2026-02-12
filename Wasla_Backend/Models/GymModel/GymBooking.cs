namespace Wasla_Backend.Models.GymModel
{
    public class GymBooking:BaseBooking
    {
        public int ServiceId { get; set; }
        public BaseService Service { get; set; }
        public string GymId { get; set; }
        [ForeignKey("GymId")]
        public Gym Gym { get; set; }
        public GymBookingStatus BookingStatus { get; set; } = GymBookingStatus.Active;
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public GymServiceType GymServiceType { get; set; }


    }
}
