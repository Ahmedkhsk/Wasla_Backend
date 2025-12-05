namespace Wasla_Backend.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }
        public int serviceDayId { get; set; }

        [ForeignKey("serviceDayId")]
        public ServiceDay serviceDay { get; set; }
        public string serviceProviderId { get; set; }
        public double price { get; set; }
        public ServiceProviderType serviceProviderType { get; set; }
        public BookingStatus bookingStatus { get; set; } = BookingStatus.upcoming;
        public WeekDayEnum newDayOfWeek { get; set; } = WeekDayEnum.None;
        public string? newStart { get; set; }
        public string? newEnd { get; set; }
        public DateOnly bookingDate { get; set; }
        public BookingType bookingType { get; set; }
        public string? imagesJson { get; set; }

        [NotMapped]
        public List<string> images
        {
            get => imagesJson == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(imagesJson);
            set => imagesJson = JsonSerializer.Serialize(value);
        }
    }
}
