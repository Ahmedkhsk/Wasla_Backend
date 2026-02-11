
namespace Wasla_Backend.Repositories.Implementation.Gyms
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(Context context) : base(context)
        {
        }

        public async Task<List<PackageInfoDto>> GetPackagesByServiceProviderId(string serviceProviderId)
        {
            return await _context.Packages.Where(p => p.ServiceProviderId == serviceProviderId && !p.IsDeleted&&!p.IsHidden)
                .Select(p => new PackageInfoDto
                {
                    Id = p.Id,
                    serviceProviderId = p.ServiceProviderId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DurationInMonths = p.DurationInMonths,
                    PhotoUrl = p.PhotoUrl
                }).ToListAsync()
                ;
        }
    }
}
