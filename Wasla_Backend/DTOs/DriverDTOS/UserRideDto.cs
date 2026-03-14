namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class UserRideDto
    {
        public int RideId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhoto { get; set; }
        public string DriverPhone { get; set; }
        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }
        public DateTime RideDate { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    }
}
