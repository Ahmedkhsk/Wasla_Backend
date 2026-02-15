
namespace Wasla_Backend.Repositories.Interfaces.Gyms
{
    public interface IGymBookingRepository:IGenericRepository<GymBooking>
    {
        public Task<List<BookingOfGym>> PackagebookingOfGym(string gymId);
        public Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status);
        public Task<List<BookingOfUser>> PackagebookingOfResident(string residentId);
        public Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status);
        public Task<bool> IsBookingExist(string residentId, int serviceId);
        Task<int> GetNumberOfBookings(string gymId);
        Task<int> GetNumOfTrainee(string gymId);
        Task<decimal> GetTotalAmount(string gymId);
        Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string gymId);
        public Task<List<UserPackageResponse>> UserPackageResponses(int serviceId);

    }
}
