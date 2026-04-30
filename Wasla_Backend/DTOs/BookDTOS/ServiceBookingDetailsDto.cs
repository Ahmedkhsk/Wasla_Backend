namespace Wasla_Backend.DTOs.BookDTOS
{
    public class ServiceBookingDetailsDto
    {
        public int id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public WeekDayEnum day { get; set; }
        public string date { get; set; }
        public BookingStatus status { get; set; }
        public string ServiceProviderName { get; set; }
        public string ServiceProviderProfilePhoto { get; set; }
        public string ServiceName { get; set; }
        public double Price { get; set; }
        public bool isPaid { get; set; }
    }
}
