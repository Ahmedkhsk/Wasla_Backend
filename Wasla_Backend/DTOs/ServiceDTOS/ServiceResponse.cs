namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class ServiceResponse
    {
        public int id { get; set; }
        public string serviceName { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public List<ServiceDayResponse> serviceDays { get; set; }
        public List<ServiceDateResponse> serviceDates { get; set; }
        public List<TimeSoltsResponse> timeSlots { get; set; }
    }
}
