namespace Wasla_Backend.Services.Implementation
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthorizationService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task CheckOwnershipByIdAsync(string ownerId)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var currentUserId = user?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                throw new UnauthorizedException(LocalizationKey.Unauthorized);

            if (currentUserId != ownerId)
                throw new ForbiddenException(LocalizationKey.NotAllowed);

            return Task.CompletedTask;
        }

        public Task CheckOwnershipByEmailAsync(string email)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var currentUserEmail = user?
                .FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(currentUserEmail))
                throw new UnauthorizedException(LocalizationKey.Unauthorized);

            if (currentUserEmail != email)
                throw new ForbiddenException(LocalizationKey.NotAllowed);

            return Task.CompletedTask;
        }
    }
}
