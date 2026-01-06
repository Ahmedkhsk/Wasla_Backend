using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }
        [HttpPost("AddFavourite")]
        public async Task<IActionResult> AddFavourite(string residentId, string serviceProviderId,  string lan = "en")
        {
            await _favouriteService.AddFavourite(residentId, serviceProviderId);
            return Ok(ResponseHelper.Success("FavouriteAddedSuccessfully", lan));
        }
        [HttpDelete("RemoveFavourite")]
        public async Task<IActionResult> RemoveFavourite(int favouriteId, string lan = "en")
        {
            await _favouriteService.RemoveFavourite(favouriteId);
            return Ok(ResponseHelper.Success("FavouriteRemovedSuccessfully", lan));
        }
        [HttpGet("GetAllFavourites")]
        public async Task<IActionResult> GetAllFavourites(string residentId, string lan = "en")
        {
            var favourites = await _favouriteService.GetAll(residentId);
            return Ok(ResponseHelper.Success("FavouritesRetrievedSuccessfully", lan, favourites));
        }
        [HttpGet("GetFavouritesByType")]
        public async Task<IActionResult> GetFavouritesByType(string residentId, [Required]ServiceProviderType serviceType, string lan = "en")
        {
            var favourites = await _favouriteService.GetByType(residentId, serviceType);
            return Ok(ResponseHelper.Success("FavouritesRetrievedSuccessfully", lan, favourites));
        }
    }
}
