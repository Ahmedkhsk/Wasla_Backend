namespace Wasla_Backend.Services.Interfaces
{
    public interface ICartService
    {
        public Task AddCart(AddCartItem dto);
        public Task RemoveCartItem(RemoveCartItemDto dto);
        public Task<List<CartItemsResponse>> GetCartItems(GetCartItems dto);
    }
}
