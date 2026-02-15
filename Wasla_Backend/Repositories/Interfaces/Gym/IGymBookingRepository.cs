namespace Wasla_Backend.Repositories.Interfaces.Gyms
{
    public interface IGymBookingRepository:IGenericRepository<GymBooking>
    {
        public Task<List<BookingOfGym>> PackagebookingOfGym(string gymId);
        public Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status);
        public Task<List<BookingOfUser>> PackagebookingOfResident(string residentId);
        public Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status);
        public Task<List<UserPackageResponse>> UserPackageResponses(GymServiceType type);
        public Task<decimal> GetTotalAmount(string id);
        public Task<int> GetNumOfTrainee(string id);
        public Task<int> GetNumberOfBookings(string id);
        public Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string id);

    }
}
