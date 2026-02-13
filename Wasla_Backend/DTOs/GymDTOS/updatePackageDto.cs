namespace Wasla_Backend.DTOs.GymDTOS
{
    public class UpdatePackageDto
    {
        public int id { get; set; }
        public MultilingualText name { get; set; }
        public MultilingualText description { get; set; }
        public GymServiceType type { get; set; }
        public decimal price { get; set; }
        public IFormFile? photo { get; set; }
        public decimal precentage { get; set; }

    }
}
