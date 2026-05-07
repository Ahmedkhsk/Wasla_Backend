namespace Wasla_Backend.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BannerController : ControllerBase
    {
        private readonly IBannersService _bannersService;

        public BannerController(IBannersService bannersService)
        {
            _bannersService = bannersService;
        }

        [HttpPost]
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> AddBanner([FromForm] AddBannerDto dto, [FromQuery] LanDto lanDto)
        {
            await _bannersService.AddBanner(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreateBanner, lanDto.lan));
        }

        [HttpGet]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> GetBanners([FromQuery] LanDto lanDto)
        {
            var banners = await _bannersService.GetBanners();
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetBanners, lanDto.lan, banners));
        }
    }
}
