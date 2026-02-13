namespace Wasla_Backend.DTOs.GymDTOS
{
    public class BookingOfUser
    {
        public int bookingId { get; set; }
        public string GymName { get; set; }
        public string imageUrl { get; set; }
        public DateTime bookingTime { get; set; }
        public int DurationInMonths { get; set; }
        public MultilingualText serviceName { get; set; }
        public GymBookingStatus bookingStatus { get; set; }
    }
}
