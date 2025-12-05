namespace Wasla_Backend.DTOs.ServiceDTOS
{
    public class ServiceDayResponse
    {
        public WeekDayEnum dayOfWeek { get; set; }
        public List<SlotsResonse> timeSlots { get; set; }
    }
}
