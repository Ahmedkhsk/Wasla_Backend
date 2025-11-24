namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class ServiceResponse
    {
        public int id { get; set; }
        public string serviceNameArabic { get; set; }
        public string descriptionArabic { get; set; }
        public string serviceNameEnglish { get; set; }
        public string descriptionEnglish { get; set; }
        public decimal price { get; set; }
        public List<ServiceDayResponse> serviceDays { get; set; }
        public List<ServiceDateResponse> serviceDates { get; set; }
        public List<TimeSoltsResponse> timeSlots { get; set; }
    }
}
