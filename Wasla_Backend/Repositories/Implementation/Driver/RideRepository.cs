namespace Wasla_Backend.Repositories.Implementation.driver
{
    public class RideRepository : GenericRepository<RideModel>, IRideRepository
    {
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public RideRepository(Context context, IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<bool> IsHasActiveRide(string residentId)
        {
            return await _context.rides.AnyAsync(r =>
                r.ResidentId == residentId &&
                (r.Status == RideStatus.InProgress ||
                 r.Status == RideStatus.Accepted ||
                 r.Status == RideStatus.Pending));
        }

        public async Task<RideDetailsForDriverDto> GetrideDetailsForDriver(int rideId)
        {
            var raw = await _context.rides
                .Where(r => r.Id == rideId)
                .Include(r => r.Resident)
                .AsNoTracking()
                .Select(r => new
                {
                    r.Resident.FullName,
                    r.Resident.PhoneNumber,
                    r.Resident.ProfilePhoto,
                    r.PickUpPlace,
                    r.DropOffPlace,
                    r.Price,
                    r.Distance
                })
                .FirstOrDefaultAsync();

            if (raw == null) return null;

            return new RideDetailsForDriverDto
            {
                ResidentName = raw.FullName,
                ResidentPhone = raw.PhoneNumber,
                ResidentImage = _fileUrlBuilderService.GetMediaUrl(raw.ProfilePhoto, MediaType.userImage),
                PickUpPlace = raw.PickUpPlace,
                DropOffPlace = raw.DropOffPlace,
                Price = raw.Price,
                Distance = raw.Distance
            };
        }

        public async Task<int> UpdateRideStatusAsync(int rideId, RideStatus accepted, string driverId)
        {
            return await _context.rides
                .Where(r => r.Id == rideId && r.Status == RideStatus.Pending)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(r => r.Status, accepted)
                    .SetProperty(r => r.DriverId, driverId));
        }

        public async Task<RideDetailsForResidentDto> GetrideDetailsForResident(int rideId)
        {
            var raw = await _context.rides
                .Where(r => r.Id == rideId)
                .Include(r => r.Driver)
                .AsNoTracking()
                .Select(r => new
                {
                    r.Driver.FullName,
                    r.Driver.DrivingExperienceYears,
                    r.Driver.Rating,
                    r.Driver.VehicleModel,
                    r.Driver.VehicleNumber,
                    r.Driver.VehicleColor,
                    r.Driver.PhoneNumber,
                    r.Driver.ProfilePhoto,
                    FirstCarImage = r.Driver.images.FirstOrDefault(),
                    r.PickUpPlace,
                    r.DropOffPlace,
                    r.Price,
                    r.RideDate
                })
                .FirstOrDefaultAsync();

            if (raw == null) return null;

            return new RideDetailsForResidentDto
            {
                DriverName = raw.FullName,
                YearsOfExperience = raw.DrivingExperienceYears,
                Rating = raw.Rating,
                VehicleModel = raw.VehicleModel,
                VehicleNumber = raw.VehicleNumber,
                VehicleColor = raw.VehicleColor.ToString(),
                VehicleImage = _fileUrlBuilderService.GetMediaUrl(raw.FirstCarImage, MediaType.DriverCarImage),
                DriverPhone = raw.PhoneNumber,
                DriverImage = _fileUrlBuilderService.GetMediaUrl(raw.ProfilePhoto, MediaType.userImage),
                PickUpPlace = raw.PickUpPlace,
                DropOffPlace = raw.DropOffPlace,
                Price = raw.Price,
                startRide = raw.RideDate
            };
        }
    }
}