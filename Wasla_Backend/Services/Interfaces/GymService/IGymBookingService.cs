namespace Wasla_Backend.Services.Interfaces.GymService
{
    public interface IGymBookingService
    {
        public Task<BookHubData> Book(GymBookDto gymBookDto);
        public Task<BookHubData> Cancel(int bookingId);
        public Task<List<BookingOfGym>> PackageBookingOFGym(string gymId);
        public Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status);
        public Task<List<BookingOfUser>> PackagebookingOfResident(string residentId);
        public Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status);
    }
}
