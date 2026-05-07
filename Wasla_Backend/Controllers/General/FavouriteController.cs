namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }
        [HttpPost("AddFavourite")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> AddFavourite(string residentId, string serviceProviderId,  string lan = "en")
        {
           var favourite= await _favouriteService.AddFavourite(residentId, serviceProviderId,lan);
            return Ok(ResponseHelper.Success(LocalizationKey.FavouriteAddedSuccessfully, lan,favourite));
        }
        [HttpDelete("RemoveFavourite")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> RemoveFavourite(int favouriteId, string lan = "en")
        {
            await _favouriteService.RemoveFavourite(favouriteId);
            return Ok(ResponseHelper.Success(LocalizationKey.FavouriteRemovedSuccessfully, lan));
        }
        [HttpGet("GetAllFavourites")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> GetAllFavourites(string residentId, string lan = "en")
        {
            var favourites = await _favouriteService.GetAll(residentId);
            return Ok(ResponseHelper.Success(LocalizationKey.FavouritesRetrievedSuccessfully, lan, favourites));
        }
        [HttpGet("GetFavouritesByType")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> GetFavouritesByType(string residentId, [Required]ServiceProviderType serviceType, string lan = "en")
        {
            var favourites = await _favouriteService.GetByType(residentId, serviceType);
            return Ok(ResponseHelper.Success(LocalizationKey.FavouritesRetrievedSuccessfully, lan, favourites));
        }
    }
}
