namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class SlotsResonse
    {
        public int id { get; set; }
        public TimeOnly start { get; set; }
        public TimeOnly end { get; set; }
        public bool isBooking { get; set; } = false;
    }
}
