
namespace Wasla_Backend.Repositories.Implementation.General
{
    public class ServiceProviderRepository : GenericRepository<ServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(Context context) : base(context)
        {
        }

        public async Task<List<ServiceProviderInfoDto>> GetAll()
        {
            var data = await (
                from sp in _context.serviceProvider

                join ur in _context.UserRoles
                    on sp.Id equals ur.UserId into urGroup
                from ur in urGroup.DefaultIfEmpty()

                join r in _context.Roles
                    on ur.RoleId equals r.Id into rGroup
                from r in rGroup.DefaultIfEmpty()

                where r == null || r.Name != "Admin"

                select new
                {
                    Sp = sp,
                    Role = r != null ? r.Name : null
                }
            ).ToListAsync();

            return data.Select(x =>
            {
                var sp = x.Sp;

                var phone = sp.Phone;

                if (sp is Gym gym)
                    phone = gym.phones?.FirstOrDefault() ?? sp.Phone;

                return new ServiceProviderInfoDto
                {
                    Id = sp.Id,
                    Name = sp.BusinessName ?? sp.FullName,
                    Email = sp.Email,
                    Description = sp.Description,
                    rating = sp.Rating,
                    Phone = phone,
                    Role = x.Role,
                    Photo = sp.ProfilePhoto

                };
            }).ToList();
        }
        public async Task<List<ServiceProviderInfoDto>> Search(string query)
        {
            var data = await (
                from sp in _context.serviceProvider

                join ur in _context.UserRoles
                    on sp.Id equals ur.UserId into urGroup
                from ur in urGroup.DefaultIfEmpty()

                join r in _context.Roles
                    on ur.RoleId equals r.Id into rGroup
                from r in rGroup.DefaultIfEmpty()

                where (r == null || r.Name != "Admin") &&

                      (
                          (sp.BusinessName != null && sp.BusinessName.Contains(query)) ||
                          (sp.FullName != null && sp.FullName.Contains(query)) ||
                          (sp.Description != null && sp.Description.Contains(query))
                      )

                select new
                {
                    Sp = sp,
                    Role = r != null ? r.Name : null
                }
            ).ToListAsync();

            return data.Select(x =>
            {
                var sp = x.Sp;

                var phone = sp.Phone;

                if (sp is Gym gym)
                    phone = gym.phones?.FirstOrDefault() ?? sp.Phone;

                return new ServiceProviderInfoDto
                {
                    Id = sp.Id,
                    Name = sp.BusinessName ?? sp.FullName,
                    Email = sp.Email,
                    Description = sp.Description,
                    rating = sp.Rating,
                    Phone = phone,
                    Role = x.Role,
                    Photo = sp.ProfilePhoto
                };
            }).ToList();
        }
    }
}
