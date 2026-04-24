namespace Wasla_Backend.Repositories.Implementation
{
    public class CartItemRepository : GenericRepository<CartItem> , ICartItemRepository
    {
        public CartItemRepository(Context context):base(context) { }

        public async Task<CartItem?> GetCartItemAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(c => c.cart)
                .FirstOrDefaultAsync(ci => ci.id == cartItemId);
        }

        public async Task<List<CartItem>> GetCartItems(GetCartItems dto)
        {
            return await _context.CartItems
                .Include(ci => ci.cart)
                .Include(ci => ci.menuItem)
                    .ThenInclude(ci => ci.category)
                .Where(ci => ci.cart.residentId == dto.residentId && ci.cart.restaurantId == dto.restaurantId)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
