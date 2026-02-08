namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IBookingHandler
    {
        public ServiceProviderType type { get; set; }
        public Task AddBooking();
        public Task RemoveBooking();
        public Task UpdateBooking();


    }
}
