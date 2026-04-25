namespace Wasla_Backend.Services.Interfaces
{
    public interface IDoctorBookService
    {
        public Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language);
        public Task UpdateBookingStatus(int bookingId, BookingStatus status,bool isResident);
        public Task UpdateBooking(UpdateBookingDto updateBookingDto);
        public Task<int> Book(BookServiceDto bookServiceDto);
    }
}
