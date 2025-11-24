namespace Wasla_Backend.Models
{
    public class Service
    {
        public int id { get; set; }
        public MultilingualText? serviceName { get; set; }
        public MultilingualText? description { get; set; }
        public decimal price { get; set; }
        public ICollection<ServiceDay> ServiceDays { get; set; }
        public ICollection<ServiceDate> ServiceDates { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; }
        public bool IsBooked { get; set; }= false;
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public Doctor doctor { get; set; }

        [ForeignKey("doctor")]
        public string doctorId { get; set; }
    }
}
