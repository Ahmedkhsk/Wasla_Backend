namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class DriverProfileDTO
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string vehicleNumber { get; set; }
        public int drivingExperienceYears { get; set; }
        public int tripsCount { get; set; }
        public VehicleType vehicleType { get; set; }
        public float rate { get; set; }
        public string birthDay { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string description { get; set; }
        public string profilePhoto { get; set; }
        public int status { get; set; }
        public List<string> carImages { get; set; }
        public List<string> driverFiles { get; set; }

    }
}
