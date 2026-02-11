namespace Wasla_Backend.DTOs.GymDTOS
{
    public class PackageInfoDto
    {
        public int Id { get; set; }
        public string serviceProviderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public string PhotoUrl { get; set; }

    }
}
