namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class RequestRideDto
    {
        public string PassengerId { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }

        public VehicleType VehicleType { get; set; }
    }
}
