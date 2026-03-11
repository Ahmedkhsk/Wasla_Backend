

namespace Wasla_Backend.Repositories.Implementation.driver
{
    public class RideRepository : GenericRepository<RideModel>, IRideRepository
    {
        public RideRepository(Context context) : base(context)
        {
        }

        public async Task<bool> IsHasActiveRide(string residentId)
        {
            return await _context.rides.AnyAsync
              (r => r.ResidentId == residentId && (r.Status == RideStatus.InProgress
              ||r.Status == RideStatus.Accepted||r.Status==RideStatus.Pending ));
        }

        public Task<RideDetailsDto> rideDetails(int rideId)
        {
            return _context.rides.Where(r => r.Id == rideId).Include(r => r.Resident).AsNoTracking()
                .Select(r => new RideDetailsDto
                {
                    ResidentName = r.Resident.FullName,
                    ResidentPhone = r.Resident.PhoneNumber,
                    PickupLatitude = r.PickupLatitude,
                    PickupLongitude = r.PickupLongitude,
                    DropoffLatitude = r.DropoffLatitude,
                    DropoffLongitude = r.DropoffLongitude,
                    RequestTime = r.RideDate,
                    Status = r.Status,
                    Price = r.Price,
                    Distance = r.Distance
                }).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateRideStatusAsync(int rideId, RideStatus accepted, string driverId)
        {
            return await _context.rides.Where(r => r.Id == rideId&&r.Status==RideStatus.Pending)
                .ExecuteUpdateAsync(s =>
                 s.SetProperty(r => r.Status, accepted)
                .SetProperty(r => r.DriverId, driverId));
        }
    }
}
