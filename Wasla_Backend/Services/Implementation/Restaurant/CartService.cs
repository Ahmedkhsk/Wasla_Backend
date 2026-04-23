namespace Wasla_Backend.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IMenuItemRepository _menuItemRepository;

        public CartService(ICartRepository cartRepo, ICartItemRepository cartItemRepo,
                           IMenuItemRepository menuItemRepository)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _menuItemRepository = menuItemRepository;
        }

        public async Task AddCart(AddCartItem dto)
        {
            if (dto.quantity <= 0)
                throw new BadRequestException(LocalizationKey.InvalidQuantity);

            var menuItem = await _menuItemRepository.GetByIdAsync(dto.menuItemId);
            if (menuItem == null)
                throw new NotFoundException(LocalizationKey.MenuItemNotFound);

            var cart = await _cartRepo.GetCartAsync(dto.residentId, dto.restaurantId);

            if (cart != null && cart.restaurantId != menuItem.restaurantId)
                throw new BadRequestException(LocalizationKey.CartDifferentRestaurantNotAllowed);

            if (cart == null)
            {
                cart = new Cart
                {
                    residentId = dto.residentId,
                    restaurantId = menuItem.restaurantId
                };

                await _cartRepo.AddAsync(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var existingItem = cart.items
                ?.FirstOrDefault(x => x.menuItemId == dto.menuItemId);

            if (existingItem != null)
            {
                existingItem.quantity += dto.quantity;
                _cartItemRepo.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    cartId = cart.id,
                    menuItemId = dto.menuItemId,
                    quantity = dto.quantity,
                    price = menuItem.price
                };

                await _cartItemRepo.AddAsync(cartItem);
            }

            await _cartItemRepo.SaveChangesAsync();
        }

        public async Task RemoveCartItem(RemoveCartItemDto dto)
        {
            var item = await _cartItemRepo.GetCartItemAsync(dto.cartItemId);

            if (item == null)
                throw new NotFoundException(LocalizationKey.CartItemNotFound);

            if (item.cart.residentId != dto.residentId)
                throw new UnauthorizedAccessException();

            _cartItemRepo.Delete(item);
            await _cartItemRepo.SaveChangesAsync();

            if (!item.cart.items.Any())
            {
                _cartRepo.Delete(item.cart);
                await _cartRepo.SaveChangesAsync();
            }
        }
        
        public async Task<List<CartItemsResponse>> GetCartItems(GetCartItems dto)
        {
            return await _cartItemRepo.GetCartItems(dto);
        }
    }
}
