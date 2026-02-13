
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
                    Precentage = p.Precentage,
                    newPrice = p.Price - (p.Price * p.Precentage / 100),
                    DurationInMonths = p.DurationInMonths,
                    PhotoUrl = p.PhotoUrl,
                    Type = p.type
                }).AsNoTracking().ToListAsync()
                ;
        }
    }
}
