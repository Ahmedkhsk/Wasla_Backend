namespace Wasla_Backend.Models
{
    [Table("DoctorBookings")]
    public class Booking : BaseBooking
    {
      
        public int serviceDayId { get; set; }

        [ForeignKey("serviceDayId")]
        public ServiceDay serviceDay { get; set; }
        public string serviceProviderId { get; set; }
        public BookingStatus bookingStatus { get; set; } = BookingStatus.upcoming;
        public WeekDayEnum newDayOfWeek { get; set; } = WeekDayEnum.none;
        public string? newStart { get; set; }
        public string? newEnd { get; set; }
        public BookingType bookingType { get; set; }
        public string? imagesJson { get; set; }
        public bool isPaymentOnline { get; set; } 

        [NotMapped]
        public List<string> images
        {
            get => imagesJson == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(imagesJson);
            set => imagesJson = JsonSerializer.Serialize(value);
        }
    }
}
