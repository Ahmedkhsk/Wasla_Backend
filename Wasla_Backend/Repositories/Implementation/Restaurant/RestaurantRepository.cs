public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(Context context) : base(context)
    {
    }

    public async Task<PagedResult<Restaurant>> GetAllRestaurants(GetGeneralWithPaginationDto<int> dto)
    {
        var query = _dbSet
                        .Where(Status => Status.Status == UserStatus.Active)
                        .AsNoTracking()
                        .AsQueryable();
        
        if(dto.id != 0)
            query = query.Where(r => r.restaurantCategoryId == dto.id);
        
        return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
    }

    public async Task<Restaurant> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Email == email);
    }

    public async Task<Restaurant> GetByUserIdAsync(string userId)
    {
        return await _dbSet.Include(r => r.restaurantCategory).FirstOrDefaultAsync(r => r.Id == userId);
    }

}
