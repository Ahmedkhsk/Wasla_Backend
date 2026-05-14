namespace Wasla_Backend.Services.Interfaces.Driver
{
    public interface IDriverService
    {
        public Task CompleteRegister(DriverCompleteRegisterDto driverCompleteRegisterDto);
        public Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id);
        public Task ChangeStatus(string driverId, DriverStatus newStatus);
        public Task TrackingDriver(TrackingDriverDto trackingDriver);
         public Task<LocationDto> GetDriverLocation(string driverId);
          public Task<List<AllNearestDriverDto>>GetTopNearestDriver(double latitude, double longitude,VehicleType vehicleType);
        public Task UpdateDriverProfile(UpdateDriverProfileDto dto);

    }
}
