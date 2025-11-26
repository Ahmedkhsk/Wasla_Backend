namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class ServiceDto
    {
        public string doctorId { get; set; }
        public MultilingualText serviceName { get; set; }
        public MultilingualText description { get; set; }
        public decimal price { get; set; }
        public List<ServiceDayDto>? serviceDays { get; set; }
        public List<ServiceDateDto>? serviceDates { get; set; }
        public List<TimeSoltsDto> timeSlots { get; set; }
    }
}
