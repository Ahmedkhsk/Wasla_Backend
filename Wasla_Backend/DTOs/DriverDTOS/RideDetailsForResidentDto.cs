namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class RideDetailsForResidentDto
    {
        public string DriverName { get; set; }
        public int YearsOfExperience { get; set; }
        public double Rating { get; set; }
        public string VehicleModel { get; set; }
        public string? VehicleNumber { get; set; }
        public string VehicleImage { get; set; }
        public string VehicleColor { get; set; }



        public string DriverPhone { get; set; }
        public string DriverImage { get; set; }
        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }
        public DateTime startRide { get; set; }
        public DateTime endRide { get; set; }
        public double Price{ get; set; }
    }
}
