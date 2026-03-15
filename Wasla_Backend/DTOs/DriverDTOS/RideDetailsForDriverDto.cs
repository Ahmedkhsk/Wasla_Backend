namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class RideDetailsForDriverDto
    {
        public string ResidentName { get; set; }
        public string ResidentPhone { get; set; }
        
        public string ResidentImage { get; set; }

        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }
        public double PickUpLatitude { get; set; }
        public double PickUpLongitude { get; set; }
        public DateTime PickUpTime { get; set; }
        public DateTime DropOffTime { get; set; }

        public double Price { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
    }
}
