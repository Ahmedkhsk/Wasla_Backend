namespace Wasla_Backend.Services.Interfaces.GymService
{
    public interface IPackageService
    {
        public Task<ServiceHubData> AddPackage(AddPackageDto addPackageDto);
        public Task<ServiceHubData> UpdatePackage(UpdatePackageDto updatePackageDto);
        public Task<ServiceHubData> RemovePackage(int PackageID);
        public Task<List<PackageInfoDto>> GetAllPackages(string serviceProviderId);
    }
}
