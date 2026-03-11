namespace Wasla_Backend.Models.Driver
{
    public class RideDispatchJob
    {
        public int Id { get; set; }

        public int RideId { get; set; }

        public string DriverId { get; set; }

        public string JobId { get; set; }
    }
}
