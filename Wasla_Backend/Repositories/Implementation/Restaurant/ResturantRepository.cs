public class ResturantRepository : GenericRepository<Restaurant>, IRestaurantRepository
{
    public ResturantRepository(Context context) : base(context)
    {
    }

    public async Task<PagedResult<Restaurant>> GetAllRestaurants(PaginationParams dto)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();
        
        return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
    }

    public async Task<Restaurant> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Email == email);
    }


}
