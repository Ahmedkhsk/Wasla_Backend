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

    }
}
