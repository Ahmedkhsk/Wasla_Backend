namespace Wasla_Backend.Models.GymModel
{
    public class Package: BaseService
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public string PhotoUrl { get; set; }
    }
}
