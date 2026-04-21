namespace Wasla_Backend.Repositories.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        public Task<CartItem?> GetCartItemAsync(int cartId);
        public Task<List<CartItemsResponse>> GetCartItems(GetCartItems dto);

    }
}
