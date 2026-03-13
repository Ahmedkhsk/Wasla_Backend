namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class DriverRideDto
    {
        public int RideId { get; set; }
        public string ResidentName { get; set; }
        public string ResidentPhone { get; set; }
        public string ResidentImage { get; set; }
        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }
        public DateTime RideDate { get; set; }

        public double Price { get; set; }
        public double Distance { get; set; }
    }
}
