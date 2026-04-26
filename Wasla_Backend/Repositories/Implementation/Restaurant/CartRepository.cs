namespace Wasla_Backend.Repositories.Implementation
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository 
    {
        public CartRepository(Context context) : base(context)
        {
        }

        public async Task<Cart?> GetCartAsync(string residentId, string restaurantId)
        {
            return await _context.Carts
                .Include(c => c.items)
                    .ThenInclude(i => i.menuItem)
                .FirstOrDefaultAsync(c => c.residentId == residentId && c.restaurantId == restaurantId);
        }

    }
}
