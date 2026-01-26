namespace Wasla_Backend.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public RoleService
            (
            IRoleRepository roleRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _roleRepository = roleRepository;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddRoleAsync(AddRoleDto roleDto)
        {
            if (await _roleRepository.RoleExistsAsync(roleDto.Value))
                throw new BadRequestException("RoleAlreadyExists");

            
            var role = new ApplicationRole
            {
                RoleName = roleDto.RoleName,
                Name = roleDto.Value,
            };

            return await _roleRepository.CreateRoleAsync(role);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                       ?? throw new NotFoundException("UserNotFound");

            return await _roleRepository.GetUserRolesAsync(user);
        }
        

        public async Task<IList<RolesResponse>> GetAllRolesAsync(string lan)
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            if (roles == null || !roles.Any())
                throw new NotFoundException("NoRolesFound");

            var rolesResult = roles.Select(rs => new RolesResponse
            {
                Id = rs.Id,
                Value = rs.Name,
                RoleName = rs.RoleName.GetText(lan)
            }).ToList();

            return rolesResult;
        }

        public async Task<IdentityResult> DeleteRole(string roleId)
        {
           var role=await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
                throw new NotFoundException("RoleNotFound");
            var result = await _roleRepository.DeleteRoleAsync(role);
            return result;
        }
    }
}
