namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminDriverDetailsDto (Driver driver)
    {
      public string Name { get; set; } = driver.FullName;
        public string Email { get; set; } = driver.Email;
        public VehicleType? VehicleType { get; set; } = driver.VehicleType;
        public string? VehicleModel { get; set; } = driver.VehicleModel;
        public string? VehicleNumber { get; set; } = driver.VehicleNumber;
        public int DrivingExperienceYears { get; set; } = driver.DrivingExperienceYears;
        public List<string>? CarImages { get; set; } = driver.images;
        public DriverStatus DriverStatus { get; set; } = driver.DriverStatus;
        public int TripsCount { get; set; } = driver.TripsCount;
    }
}
