namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class RideDetailsForDriverDto
    {
        public string ResidentName { get; set; }
        public string ResidentPhone { get; set; }
        
        public string ResidentImage { get; set; }

        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }

        public double Price { get; set; }
        public double Distance { get; set; }
    }
}
