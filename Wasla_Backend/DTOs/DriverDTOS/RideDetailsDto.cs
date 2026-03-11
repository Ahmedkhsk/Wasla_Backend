namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class RideDetailsDto
    {
        public string ResidentName { get; set; }
        public string ResidentPhone { get; set; }

        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
        public DateTime RequestTime { get; set; }
        public RideStatus Status { get; set; }
        public double Price { get; set; }
        public double Distance { get; set; }
    }
}
