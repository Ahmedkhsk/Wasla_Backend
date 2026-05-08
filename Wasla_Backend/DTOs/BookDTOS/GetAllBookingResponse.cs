namespace Wasla_Backend.DTOs.BookDTOS
{
    public class GetAllBookingResponse
    {
        public int bookingId { get; set; }
        public string serviceName { get; set; }
        public string userName { get; set; }
        public string userImage { get; set; }
        public DateTime date { get; set; }
        public BookingStatus status { get; set; }
        public TimeOnly start { get; set; }
        public TimeOnly end { get; set; }
        public bool isPaid { get; set; }
        public WeekDayEnum day { get; set; }
        public BookingType bookingType { get; set; }
        public string phone { get; set; }
        public decimal price { get; set; }
        public List<string>? bookingImages { get; set; }
    }
}
