public class ResturantRepository : GenericRepository<Restaurant>, IRestaurantRepository
{
    public ResturantRepository(Context context) : base(context)
    {
    }

    public async Task<Restaurant> GetByEmail(string email)
    {
        return await _context.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Email == email);
    }

    public async Task<Restaurant> GetByName(string name)
    {
     return await _context.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.BusinessName == name);

    }

    public async Task<Restaurant> GetByOwnerName(string ownerName)
    {
        return await _context.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.OwnerName == ownerName);
    }

    public  bool IsEmailExists(string email)
    {
        return  _context.Restaurants.AsNoTracking().Any(r => r.Email == email);
    }

    public bool IsNameExists(string name)
    {
        return _context.Restaurants.AsNoTracking().Any(r => r.BusinessName == name);
    }

    public bool IsOwnerExist(string ownerName)
    {
        return _context.Restaurants.AsNoTracking().Any(r => r.OwnerName == ownerName);
    }
}
