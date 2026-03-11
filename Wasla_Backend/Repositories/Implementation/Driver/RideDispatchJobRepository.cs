

namespace Wasla_Backend.Repositories.Implementation.Driver
{
    public class RideDispatchJobRepository:GenericRepository<RideDispatchJobModel>, IRideDispatchJobRepository
    {
        public RideDispatchJobRepository(Context context) : base(context)
        {
        }

        public async Task DeleteByRideIdAsync(int rideId)
        {
             await _context.RideDispatchJobs.Where(r => r.RideId == rideId).ExecuteDeleteAsync();
        }

        public async Task<List<RideDispatchJobModel>> GetByRideIdAsync(int rideId)
        {
            return await _context.RideDispatchJobs.Where(r=>r.RideId==rideId).ToListAsync();
        }

    }
}
