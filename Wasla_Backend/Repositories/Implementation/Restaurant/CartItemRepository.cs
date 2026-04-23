namespace Wasla_Backend.Repositories.Implementation
{
    public class CartItemRepository : GenericRepository<CartItem> , ICartItemRepository
    {
        public CartItemRepository(Context context):base(context) { }

        public async Task<CartItem?> GetCartItemAsync(int cartId)
        {
            return await _context.CartItems
                .Include(c => c.cart)
                .FirstOrDefaultAsync(ci => ci.cartId == cartId);
        }

        public async Task<List<CartItemsResponse>> GetCartItems(GetCartItems dto)
        {
            return await _context.CartItems
                .Include(ci => ci.cart)
                .Where(ci => ci.cart.residentId == dto.residentId && ci.cart.restaurantId == dto.restaurantId)
                .Select(ci => new CartItemsResponse
                {
                    cartItemId = ci.id,
                    menuItemId = ci.menuItemId,
                    quantity = ci.quantity,
                    totalPrice = ci.price * ci.quantity
                })
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
