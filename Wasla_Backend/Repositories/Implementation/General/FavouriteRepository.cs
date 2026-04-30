using Microsoft.EntityFrameworkCore;
using Wasla_Backend.Data;
using Wasla_Backend.DTOs.FavouritsDTOS;
using Wasla_Backend.Models;
using Wasla_Backend.Repositories.Interfaces;

namespace Wasla_Backend.Repositories.Implementation
{
    public class FavouriteRepository :GenericRepository<Favourites>, IFavouriteRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public FavouriteRepository(IServiceScopeFactory scopeFactory,Context context):base(context)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<List<ServiceProviderFavourite>> GetAllAsync(string residentId)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Context>();

            var data = await db.Favorite
                .Include(f => f.ServiceProvider)
                .Where(f => f.UserId == residentId)
                .OrderByDescending(f=>f.CreatedAt)
                .ToListAsync();

            var result = data.Select(f => new ServiceProviderFavourite
            {
                id = f.Id,
                residentId = f.UserId,
                serviceProviderId = f.ServiceProviderId,

                serviceProviderName =
                    f.ServiceProvider is Doctor d ? d.FullName :
                    f.ServiceProvider is Gym g ? g.BusinessName :
                    f.ServiceProvider.FullName,

                serviceProviderProfilePhoto = f.ServiceProvider.ProfilePhoto,

                serviceProviderPhone =
                    f.ServiceProvider is Gym gym
                        ? gym.phones?.FirstOrDefault()
                        : f.ServiceProvider.Phone,

                ServiceProviderType = f.ServiceType.ToString()
            }).ToList();

            return result;
        }

        public async Task<List<ServiceProviderFavourite>> GetByTypeAsync(
    string residentId,
    ServiceProviderType serviceType)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Context>();

            var data = await db.Favorite
                .Include(f => f.ServiceProvider)
                .Where(f => f.UserId == residentId && f.ServiceType == serviceType)
                .OrderByDescending(f => f.CreatedAt)

                .ToListAsync();  

            var result = data.Select(f => new ServiceProviderFavourite
            {
                id = f.Id,
                residentId = f.UserId,
                serviceProviderId = f.ServiceProviderId,

                serviceProviderName =
                    f.ServiceProvider is Doctor d ? d.FullName :
                    f.ServiceProvider is Gym g ? g.BusinessName :
                    f.ServiceProvider.FullName,

                serviceProviderProfilePhoto = f.ServiceProvider.ProfilePhoto,

                serviceProviderPhone =
                    f.ServiceProvider is Gym gym
                        ? gym.phones?.FirstOrDefault()
                        : f.ServiceProvider.Phone,

                ServiceProviderType = f.ServiceType.ToString()
            }).ToList();

            return result;
        }

    }
}
