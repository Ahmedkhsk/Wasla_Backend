namespace Wasla_Backend.Services.Interfaces
{
    public interface IBookService
    {
        public Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language);
        public Task UpdateBookingStatus(int bookingId, BookingStatus status);
        public Task UpdateBookingStatus(UpdateBookingDto updateBookingDto);
        public Task Book(BookServiceDto bookServiceDto);
    }
}
