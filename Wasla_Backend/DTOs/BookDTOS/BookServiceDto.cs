namespace Wasla_Backend.DTOs.BookDTOS
{
    public class BookServiceDto
    {
        public string userId { get; set; }
        public int serviceDayId { get; set; }
        public int serviceId { get; set; }
        public string serviceProviderId { get; set; }
        public double price { get; set; }
        public DateOnly bookingDate { get; set; }
        public BookingType bookingType { get; set; }
        public ServiceProviderType serviceProviderType { get; set; }
        public bool isPaymentOnline { get; set; }
        public List<IFormFile>? images { get; set; }
    }
}
