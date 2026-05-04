namespace Wasla_Backend.Services.Implementation.General
{
    public class BannersService : IBannersService
    {
        private readonly IGenericRepository<Banner> _bannerRepo;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IFileService _fileService;

        public BannersService(IGenericRepository<Banner> bannerRepo,
                              IFileUrlBuilderService fileUrlBuilderService,
                              IFileService fileService)
        {
            _bannerRepo = bannerRepo;
            _fileUrlBuilderService = fileUrlBuilderService;
            _fileService = fileService;
        }

        public async Task AddBanner(AddBannerDto dto)
        {
            var image = await _fileService.AddFileAsync(dto.image, _fileUrlBuilderService.GetPath(MediaType.bannerImage));

            if (string.IsNullOrEmpty(image))
                throw new BadRequestException(LocalizationKey.InvalidImage);

            var banner = new Banner
            {
                image = image,
                description = dto.description,
                title = dto.title
            };
            await _bannerRepo.AddAsync(banner);
            await _bannerRepo.SaveChangesAsync();
        }

        public async Task<List<GetBannersResponse>> GetBanners()
        {
            var banners = await _bannerRepo.GetAllAsync();
            return banners.Select(b => new GetBannersResponse
            {
                id = b.id,
                imageUrl = _fileUrlBuilderService.GetMediaUrl(b.image, MediaType.bannerImage),
                description = b.description,
                title = b.title
            }).ToList();
        }
    }
}
