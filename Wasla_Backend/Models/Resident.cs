namespace Wasla_Backend.Models
{
    public class Resident : ApplicationUser
    {
        public string? NationalId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
}
