namespace Wasla_Backend.Repositories.Interfaces.driver
{
    public interface IRideDispatchJobRepository:IGenericRepository<RideDispatchJobModel>
    {
        public Task<List<RideDispatchJobModel>> GetByRideIdAsync(int rideId);
        public Task DeleteByRideIdAsync(int rideId);
    }
}
