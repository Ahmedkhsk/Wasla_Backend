namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class UpdateDriverProfileDto
    {
        public string Id { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }

        public VehicleType? VehicleType { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleNumber { get; set; }

        public int? DrivingExperienceYears { get; set; }
        public string? Description { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public IFormFile? Photo { get; set; }

        public VehicleColor? VehicleColor { get; set; }

        public List<IFormFile>? CarImages { get; set; }

        public List<IFormFile>? DriverFiles { get; set; }
    }
}
