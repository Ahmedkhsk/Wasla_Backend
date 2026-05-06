namespace Wasla_Backend.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public CartService(ICartRepository cartRepo, ICartItemRepository cartItemRepo,
                           IMenuItemRepository menuItemRepository, IFileUrlBuilderService fileUrlBuilderService, 
                           IUserAuthorizationService userAuthorizationService)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _menuItemRepository = menuItemRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
            _userAuthorizationService = userAuthorizationService;
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

            await _userAuthorizationService.CheckOwnershipByIdAsync(item.cart.residentId);

            _cartItemRepo.Delete(item);
            await _cartItemRepo.SaveChangesAsync();
        }

        public async Task<List<CartItemsResponse>> GetCartItems(GetCartItems dto)
        {
            var cartItems = await _cartItemRepo.GetCartItems(dto);

            return cartItems.Select(ci => new CartItemsResponse
            {
                cartItemId = ci.id,
                menuItemId = ci.menuItemId,
                menuItemCategoryName = ci.menuItem.category.name.GetText(dto.lan),
                quantity = ci.quantity,
                totalPrice = ci.price * ci.quantity,
                menuItemName = ci.menuItem.name.GetText(dto.lan),
                imageUrl = _fileUrlBuilderService.GetMediaUrl(ci.menuItem.imageUrl, MediaType.restaurantImage)
            }).ToList();
        }
        public async Task UpdateQuantity(UpdateQuantityDto dto)
        {
            if (dto.quantity <= 0)
                throw new BadRequestException(LocalizationKey.InvalidQuantity);
            var item = await _cartItemRepo.GetCartItemAsync(dto.cartItemId);
            if (item == null)
                throw new NotFoundException(LocalizationKey.CartItemNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(item.cart.residentId);
            item.quantity = dto.quantity;
            _cartItemRepo.Update(item);
            await _cartItemRepo.SaveChangesAsync();
        }
    }
}
