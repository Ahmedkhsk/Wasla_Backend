namespace Wasla_Backend.Extensions
{
    public static class ClaimsPrincipalHelper
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? throw new UnauthorizedAccessException("User ID not found in token");
        }
    }
}