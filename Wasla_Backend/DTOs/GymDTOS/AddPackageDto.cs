namespace Wasla_Backend.DTOs.GymDTOS
{
    public class AddPackageDto
    {
        public string serviceProviderId { get; set; }
        
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int durationInMonths { get; set; }
        public IFormFile photo { get; set; }
        
    }
}
