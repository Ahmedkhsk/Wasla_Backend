namespace Wasla_Backend.Services.Implementation
{
    public class MenuItemCategoryService : IMenuItemCategoryService
    {
        private readonly IMenuItemCategoryRepository _menuItemCategoryRepo;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public MenuItemCategoryService
            (IMenuItemCategoryRepository MenuItemCategoryRepo,
            IRestaurantRepository restaurantRepository,IMenuItemRepository menuItemRepository,
            IUserAuthorizationService userAuthorizationService)
        {
            _menuItemCategoryRepo = MenuItemCategoryRepo;
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task AddCategory(AddMenuItemCategoryDto dto)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(dto.restaurantId);
            if (restaurant == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var category = new MenuItemCategory
            {
                name = dto.name,
                restaurantId = dto.restaurantId,
            };

            await _menuItemCategoryRepo.AddAsync(category);
            await _menuItemCategoryRepo.SaveChangesAsync();
        }

        public async Task UpdateCategory(UpdateMenuItemCategoryDto dto)
        {
            var category = await _menuItemCategoryRepo.GetByIdAsync(dto.id);

            if (category == null)
                throw new NotFoundException(LocalizationKey.MenuItemCategoryNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(category.restaurantId);

            category.name = dto.name;

            _menuItemCategoryRepo.Update(category);
            await _menuItemCategoryRepo.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _menuItemCategoryRepo.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException(LocalizationKey.MenuItemCategoryNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(category.restaurantId);

            var hasItems = await _menuItemRepository.AnyAsync(x => x.categoryId == id);

            if (hasItems)
                throw new BadRequestException(LocalizationKey.CategoryHasItems);


            _menuItemCategoryRepo.Delete(category);
            await _menuItemCategoryRepo.SaveChangesAsync();
        }

        public async Task<List<GetMenuItemCategoryDto>> GetMenuItemCategoryDtos(GetGeneralDto<string> dto)
        {
            return await _menuItemCategoryRepo.GetMenuItemCategory(dto);
        }
    }
}
