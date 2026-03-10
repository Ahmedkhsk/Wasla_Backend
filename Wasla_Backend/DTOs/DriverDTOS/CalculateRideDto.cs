namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class CalculateRideDto
    {
        public double PickupLatitude { get; set; }

        public double PickupLongitude { get; set; }

        public double DropoffLatitude { get; set; }

        public double DropoffLongitude { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
