namespace Wasla_Backend.Models
{
    [Table("DoctorServices")]
    public class Service
    {
        public int id { get; set; }
        public MultilingualText? serviceName { get; set; }
        public MultilingualText? description { get; set; }
        public decimal price { get; set; }
        public ICollection<ServiceDay>? ServiceDays { get; set; }
        public Doctor doctor { get; set; }

        [ForeignKey("doctor")]
        public string doctorId { get; set; }

        public bool isDelete { get; set; } = false;
    }
}
