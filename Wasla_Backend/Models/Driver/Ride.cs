namespace Wasla_Backend.Models.Driver
{
    public class ride : BaseBooking

    {
        public string? DriverId { get; set; }

        [ForeignKey("DriverId")]

        public Driver? Driver { get; set; }

        public double PickupLatitude { get; set; }
        public string PickUpPlace { get; set; }
        public string DropOffPlace { get; set; }

        public double PickupLongitude { get; set; }

        public double DropoffLatitude { get; set; }

        public double DropoffLongitude { get; set; }


        public double Distance { get; set; }

        public RideStatus Status { get; set; } = RideStatus.Pending;


    }
}
