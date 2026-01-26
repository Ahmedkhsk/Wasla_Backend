namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IRefreshTokenRepository: IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
        Task DeleteTokensByUserIdAsync(string userId);
        Task<IEnumerable<RefreshToken>> GetTokensByUserIdAsync(string userId);
        Task<RefreshToken> GetTokensByUserIdAndTokenAsync(string userId, string token);
    }
}
