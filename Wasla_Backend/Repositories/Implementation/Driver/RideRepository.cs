

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
                    r.Distance,
                    r.PickupLatitude, r.PickupLongitude,r.RideDate
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
                PickUpLatitude=raw.PickupLatitude,
                PickUpLongitude = raw.PickupLongitude,
                PickUpTime=raw.RideDate,
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
                    r.DriverId,
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
                DriverID = raw.DriverId,
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

        public async Task<List<UserRideDto>> GetUserRides(string residentId)
        {
           return await _context.rides
                .Where(r => r.ResidentId == residentId)
                .OrderByDescending(r => r.RideDate)
                .Include(r => r.Driver)
                .Select(r => new UserRideDto
                {
                    RideId = r.Id,
                    DriverName = r.Driver.FullName,
                    DriverPhoto = _fileUrlBuilderService.GetMediaUrl(r.Driver.ProfilePhoto, MediaType.userImage),
                    DriverPhone = r.Driver.PhoneNumber,
                    PickUpPlace = r.PickUpPlace,
                    DropOffPlace = r.DropOffPlace,
                    Price = r.Price,
                    RideDate = r.RideDate,
                    Status = r.Status.ToString()

                })
                .ToListAsync();
        }

        public async Task<List<DriverRideDto>> GetDriverRides(string driverId)
        {
            return await _context.rides
                .Where(r => r.DriverId == driverId )
                .OrderByDescending(r => r.RideDate)
                .Include(r => r.Resident)
                .Select(r => new DriverRideDto
                {
                    RideId = r.Id,
                    ResidentName = r.Resident.FullName,
                    ResidentPhone = r.Resident.PhoneNumber,
                    ResidentImage = _fileUrlBuilderService.GetMediaUrl(r.Resident.ProfilePhoto, MediaType.userImage),
                    PickUpPlace = r.PickUpPlace,
                    DropOffPlace = r.DropOffPlace,
                    RideDate = r.RideDate,
                    Price = r.Price,
                    Distance = r.Distance,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<DriverChartDto> GetDriverChart(string driverId)
        {
            var ridesQuery = _context.rides
                .AsNoTracking()
                .Where(r => r.DriverId == driverId && r.Status == RideStatus.Completed);

            var numberOfRides = await ridesQuery.CountAsync();

            var numberOfDeliveredResident = await ridesQuery
                .Select(r => r.ResidentId)
                .Distinct()
                .CountAsync();

            var totalAmount = await ridesQuery.SumAsync(r => r.Price);

            var years = await ridesQuery
                .GroupBy(r => r.RideDate.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(r => r.RideDate.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = monthGroup.Sum(r => r.Price)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();

            return new DriverChartDto
            {
                numberOfRides = numberOfRides,
                numberOfDeliveredResident = numberOfDeliveredResident,
                totalAmount = totalAmount,
                years = years
            };
        }



    }
}