namespace Wasla_Backend.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannersService _bannersService;

        public BannerController(IBannersService bannersService)
        {
            _bannersService = bannersService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBanner([FromForm] AddBannerDto dto, [FromQuery] LanDto lanDto)
        {
            await _bannersService.AddBanner(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreateBanner, lanDto.lan));
        }

        [HttpGet]
        public async Task<IActionResult> GetBanners([FromQuery] LanDto lanDto)
        {
            var banners = await _bannersService.GetBanners();
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetBanners, lanDto.lan, banners));
        }
    }
}
