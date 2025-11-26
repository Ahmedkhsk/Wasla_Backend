namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class UpdateServiceDto
    {
        public int serviceId { get; set; }
        public MultilingualText serviceName { get; set; }
        public MultilingualText description { get; set; }
        public decimal price { get; set; }
        public List<ServiceDayDto>? serviceDays { get; set; }
        public List<ServiceDateDto>? serviceDates { get; set; }
        public List<TimeSoltsDto> timeSlots { get; set; }

    }
}
