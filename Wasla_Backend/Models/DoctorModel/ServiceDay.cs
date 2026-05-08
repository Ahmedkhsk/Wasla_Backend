namespace Wasla_Backend.Models
{
    public class ServiceDay
    {
        public int id { get; set; }
        public WeekDayEnum dayOfWeek { get; set; }
        public TimeOnly start { get; set; }
        public TimeOnly end { get; set; }
        public bool isBooking { get; set; } = false;
        public Service service { get; set; }

        [ForeignKey("service")]
        public int serviceId { get; set; }
    }
}
