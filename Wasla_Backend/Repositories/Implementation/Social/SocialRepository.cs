namespace Wasla_Backend.Repositories.Implementation
{
    public class SocialRepository : GenericRepository<Social>, ISocialRepository
    {
        public SocialRepository(Context context) : base(context)
        {
        }

        public async Task<Social?> GetSocialById(int id)
        {
            return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.id == id);
        }
    }
}
