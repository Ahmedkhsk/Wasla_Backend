namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class AddServiceDto
    {
        public string doctorId { get; set; }
        public MultilingualText serviceName { get; set; }
        public MultilingualText description { get; set; }
        public decimal price { get; set; }
        public List<AddServiceDayDto>? serviceDays { get; set; }
        public List<AddServiceDateDto>? serviceDates { get; set; }
        public List<AddTimeSoltsDto> timeSlots { get; set; }
    }
}
