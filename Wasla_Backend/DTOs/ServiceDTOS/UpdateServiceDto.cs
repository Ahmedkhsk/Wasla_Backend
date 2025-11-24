namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class UpdateServiceDto
    {
        public int serviceId { get; set; }
        public MultilingualText serviceName { get; set; }
        public MultilingualText description { get; set; }
        public decimal price { get; set; }
        public List<ServiceDayResponse>? serviceDays { get; set; }
        public List<ServiceDateResponse>? serviceDates { get; set; }
        public List<TimeSoltsResponse> timeSlots { get; set; }

    }
}
