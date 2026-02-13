namespace Wasla_Backend.DTOs.GymDTOS
{
    public class PackageInfoDto
    {
        public int Id { get; set; }
        public string serviceProviderId { get; set; }
        public MultilingualText Name { get; set; }
        public MultilingualText Description { get; set; }
        public decimal Precentage { get; set; }
        public decimal Price { get; set; }
        public decimal newPrice { get; set; }
        public int DurationInMonths { get; set; }
        public string PhotoUrl { get; set; }
        public GymServiceType Type { get; set; }

    }
}
