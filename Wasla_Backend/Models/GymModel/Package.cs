namespace Wasla_Backend.Models.GymModel
{
    public class Package: BaseService
    {
        public MultilingualText Name { get; set; }
        public MultilingualText Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Precentage { get; set; }
        public GymServiceType type { get; set; }
    }
}
