namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IBannersService
    {
        public Task AddBanner(AddBannerDto dto);
        public Task<List<GetBannersResponse>> GetBanners();
    }
}
