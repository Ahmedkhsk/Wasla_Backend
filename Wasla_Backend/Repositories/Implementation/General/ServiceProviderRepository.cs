namespace Wasla_Backend.Repositories.Implementation.General
{
    public class ServiceProviderRepository
        : GenericRepository<ServiceProvider>, IServiceProviderRepository
    {
        public ServiceProviderRepository(Context context) : base(context)
        {
        }

     
        public async Task<PagedResult<ServiceProviderInfoDto>> GetAll(
            int pageNumber,
            int pageSize)
        {
            var query = (
                from sp in _context.serviceProvider
                where sp.Status == 0
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
            );

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Sp.Rating)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = data.Select(x =>
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

            return new PagedResult<ServiceProviderInfoDto>
            {
                Data = result,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

       
        public async Task<PagedResult<ServiceProviderInfoDto>> Search(
            string queryText,
            int pageNumber,
            int pageSize)
        {
            var query = (
                from sp in _context.serviceProvider
                where sp.Status == 0
                join ur in _context.UserRoles
                    on sp.Id equals ur.UserId into urGroup
                    where sp.Status == 0
                from ur in urGroup.DefaultIfEmpty()

                join r in _context.Roles
                    on ur.RoleId equals r.Id into rGroup
                from r in rGroup.DefaultIfEmpty()

                where (r == null || r.Name != "Admin") &&
                      (
                          (sp.BusinessName != null && sp.BusinessName.Contains(queryText)) ||
                          (sp.FullName != null && sp.FullName.Contains(queryText)) ||
                          (sp.Description != null && sp.Description.Contains(queryText))
                      )

                select new
                {
                    Sp = sp,
                    Role = r != null ? r.Name : null
                }
            );

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Sp.Rating)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = data.Select(x =>
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

            return new PagedResult<ServiceProviderInfoDto>
            {
                Data = result,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}