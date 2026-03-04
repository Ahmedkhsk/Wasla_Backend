namespace Wasla_Backend.Services.Interfaces.Driver
{
    public interface IDriverService
    {
        public Task CompleteRegister(DriverCompleteRegisterDto driverCompleteRegisterDto);
        public Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id);
        public Task ChangeStatus(string driverId, DriverStatus newStatus);


    }
}
