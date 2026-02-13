namespace Wasla_Backend.DTOs.GymDTOS
{
    public class AddPackageDto
    {
        public string serviceProviderId { get; set; }
        
        public MultilingualText name { get; set; }
        public MultilingualText description { get; set; }
        public decimal price { get; set; }
        public int durationInMonths { get; set; }
        public decimal precentage { get; set; }
        public GymServiceType type { get; set; }
        public IFormFile photo { get; set; }
        
    }
}
