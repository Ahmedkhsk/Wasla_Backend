using Wasla_Backend.Repositories.Interfaces.Authentication;

namespace Wasla_Backend.Repositories.Implementation.Authentication
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly IUserRepository _userRepository;
        public AdminRepository(Context context, IUserRepository userRepository) : base(context)
        {
            _userRepository = userRepository;
        }
        public async Task<List<AdminResponseDto>> GetAllAdminsAsync()
        {
            return await _context.Admins
                .Where(a => !a.IsDeleted)
                .Select(a => new AdminResponseDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Status = a.Status
                })
                .ToListAsync();
        }
    }
}
