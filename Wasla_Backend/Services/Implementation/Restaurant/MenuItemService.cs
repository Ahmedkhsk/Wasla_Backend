namespace Wasla_Backend.Services.Implementation
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemCategoryRepository _menuItemCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IFileService _fileService;

        public MenuItemService
            (IMenuItemRepository menuItemRepository, IRestaurantRepository restaurantRepository,
            IMenuItemCategoryRepository menuItemCategoryRepository, IMapper mapper,
            IFileUrlBuilderService fileUrlBuilderService, IFileService fileService)
        {
            _menuItemRepository = menuItemRepository;
            _restaurantRepository = restaurantRepository;
            _menuItemCategoryRepository = menuItemCategoryRepository;
            _mapper = mapper;
            _fileUrlBuilderService = fileUrlBuilderService;
            _fileService = fileService;
        }

        public async Task AddItem(AddMenuItemDto dto)
        {
            var restaurant = await _restaurantRepository.GetByUserIdAsync(dto.restaurantId);
            if (restaurant == null)
                throw new NotFoundException(LocalizationKey.RestaurantNotFound);

            var category = await _menuItemCategoryRepository.GetByIdAsync(dto.categoryId);
            if (category == null)
                throw new NotFoundException(LocalizationKey.MenuItemCategoryNotFound);

            var menuItem = _mapper.Map<MenuItem>(dto);

            var image = await _fileService.AddFileAsync(dto.imageUrl,
                        _fileUrlBuilderService.GetPath(MediaType.restaurantImage));

            menuItem.imageUrl = image;
            await _menuItemRepository.AddAsync(menuItem);
            await _menuItemRepository.SaveChangesAsync();
        }

        public async Task UpdateItem(UpdateMenuItemDto dto)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(dto.id);
            if (menuItem == null)
                throw new NotFoundException(LocalizationKey.MenuItemNotFound);

            _mapper.Map(dto, menuItem);
            if (dto.imageUrl != null)
            {
                var image = await _fileService.ReplaceFileAsync(menuItem.imageUrl, dto.imageUrl,
                    _fileUrlBuilderService.GetPath(MediaType.restaurantImage));

                menuItem.imageUrl = image;
            }
            _menuItemRepository.Update(menuItem);
            await _menuItemRepository.SaveChangesAsync();
        }

        public async Task DeleteItem(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
                throw new NotFoundException(LocalizationKey.MenuItemNotFound);

            _fileService.DeleteFile(menuItem.imageUrl,
                    _fileUrlBuilderService.GetPath(MediaType.restaurantImage));

            _menuItemRepository.Delete(menuItem);
            await _menuItemRepository.SaveChangesAsync();
        }

    }
}
