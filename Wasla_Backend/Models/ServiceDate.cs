namespace Wasla_Backend.Models
{
    public class ServiceDate
    {
        public int id { get; set; }
        public string date { get; set; }
        public Service service { get; set; }

        [ForeignKey("service")]
        public int serviceId { get; set; }
    }
}
