
namespace Wasla_Backend.Repositories.Implementation
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(Context context) : base(context)
        {
        }
        public async Task DeleteTokensByUserIdAsync(string userId)
        {
            var tokens = await  _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var tokenEntity =await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            return tokenEntity;
        }

        public async Task<RefreshToken> GetTokensByUserIdAndTokenAsync(string userId, string token)
        {
            var tokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token);
            return tokenEntity;
        }

        public async Task<IEnumerable<RefreshToken>> GetTokensByUserIdAsync(string userId)
        {
            var tokens = await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            return tokens;

        }

       


    }
}
