namespace Wasla_Backend.DTOs.BookDTOS
{
    public class GetAllBookingResponse
    {
        public int bookingId { get; set; }
        public string serviceName { get; set; }
        public string userName { get; set; }
        public string userImage { get; set; }
        public string date { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int day { get; set; }
        public BookingType bookingType { get; set; }
        public string phone { get; set; }
        public decimal price { get; set; }
        public List<string>? bookingImages { get; set; }
    }
}
