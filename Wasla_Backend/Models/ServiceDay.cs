namespace Wasla_Backend.Models
{
    public class ServiceDay
    {
        public int id { get; set; }
        public int dayOfWeek { get; set; }
        public Service service { get; set; }

        [ForeignKey("service")]
        public int serviceId { get; set; }
    }
}
