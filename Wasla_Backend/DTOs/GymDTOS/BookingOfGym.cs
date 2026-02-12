namespace Wasla_Backend.DTOs.GymDTOS
{
    public class BookingOfGym
    {
        public int bookingId { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public DateTime bookingTime { get; set; }
        public int DurationInMonths { get; set; }
        public string serviceName { get; set; }
        public GymBookingStatus bookingStatus { get; set; }

    }
}
