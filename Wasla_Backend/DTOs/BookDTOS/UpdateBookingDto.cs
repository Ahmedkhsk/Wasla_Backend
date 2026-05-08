namespace Wasla_Backend.DTOs.BookDTOS
{
    public class UpdateBookingDto
    {
        public int BookingId { get; set; }
        public WeekDayEnum newDayOfWeek { get; set; }
        public TimeOnly newStart { get; set; }
        public TimeOnly newEnd { get; set; }
        public DateTime bookingDate { get; set; }
    }
}
