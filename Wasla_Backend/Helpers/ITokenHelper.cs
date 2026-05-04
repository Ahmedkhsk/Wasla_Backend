namespace Wasla_Backend.Helpers
{
    public interface ITokenHelper
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
        string GenerateRefreshToken();
    }
}
