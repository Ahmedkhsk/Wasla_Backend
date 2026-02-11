namespace Wasla_Backend.Repositories.Interfaces.Gyms
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
       public Task<List<PackageInfoDto>> GetPackagesByServiceProviderId(string serviceProviderId);
    }
}
