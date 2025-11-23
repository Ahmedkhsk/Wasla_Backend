namespace Wasla_Backend.Models
{
    public class TimeSlot
    {
        public int id { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }

        public Service service { get; set; }

        [ForeignKey("service")]
        public int serviceId { get; set; }
    }
}
