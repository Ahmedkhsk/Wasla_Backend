namespace Wasla_Backend.Models.Driver
{
    public class Driver : ServiceProvider
    {
        public VehicleType? VehicleType { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public int DrivingExperienceYears { get; set; }
        public string? CarImages { get; set; }
        public DriverStatus DriverStatus { get; set; }=DriverStatus.Offline;
        public int TripsCount { get; set; }= 0;

        [NotMapped]
        public List<string>? images
        {
            get => CarImages == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(CarImages);
            set => CarImages = JsonSerializer.Serialize(value);
        }
        public string? DriverFilesJson { get; set; }

        [NotMapped]
        public List<string>? DriverFiles
        {
            get => string.IsNullOrEmpty(DriverFilesJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(DriverFilesJson)!;

            set => DriverFilesJson = JsonSerializer.Serialize(value);
        }


    }
}
