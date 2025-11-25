using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("service-provider/{serviceProviderId}")]
        public async Task<IActionResult> GetReviewsByServiceProviderId(string serviceProviderId,string lan="en")
        {
            var reviews = await _reviewService.GetReviewsByServiceProviderIdAsync(serviceProviderId);
            return Ok(ResponseHelper.Success("GetReviewsSuccess",lan,reviews));
        }
        [HttpGet("rating/{rating}")]
        public async Task<IActionResult> GetReviewsByRating(int rating, string lan = "en")
        {
            var reviews = await _reviewService.GetReviewsByRating(rating);
            return Ok(ResponseHelper.Success("GetReviewsSuccess", lan, reviews));
        }
    }
}
