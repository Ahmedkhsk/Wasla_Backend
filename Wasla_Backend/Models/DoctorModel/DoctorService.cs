namespace Wasla_Backend.Models
{
    [Table("DoctorServices")]
    public class Service : BaseService
    {
        public MultilingualText? serviceName { get; set; }
        public MultilingualText? description { get; set; }
        public decimal price { get; set; }
        public ICollection<ServiceDay>? ServiceDays { get; set; }
   
    }
}
