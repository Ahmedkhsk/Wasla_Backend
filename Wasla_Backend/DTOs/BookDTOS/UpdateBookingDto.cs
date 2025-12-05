namespace Wasla_Backend.DTOs.BookDTOS
{
    public class UpdateBookingDto
    {
        public int BookingId { get; set; }
        public WeekDayEnum newDayOfWeek { get; set; }
        public string newStart { get; set; }
        public string newEnd { get; set; }
        public DateOnly bookingDate { get; set; }
    }
}
