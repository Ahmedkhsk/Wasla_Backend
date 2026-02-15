namespace Wasla_Backend.Services.Interfaces.GymService
{
    public interface IGymBookingService
    {
        public Task<BookResponse> Book(GymBookDto gymBookDto, string lan);
        public Task<BookHubData> Cancel(int bookingId);
        public Task<List<BookingOfGym>> PackageBookingOFGym(string gymId);
        public Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status);
        public Task<List<BookingOfUser>> PackagebookingOfResident(string residentId);
        public Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status);
        public Task<ChartsResponse> chartsResponse(string gymId);
        public Task<List<UserPackageResponse>> UserPackageResponses(GymServiceType type);
        public Task<QrValidationResult> ValidateQrAsync(int bookingId);
    }
}
