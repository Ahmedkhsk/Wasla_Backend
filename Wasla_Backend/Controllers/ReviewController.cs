using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wasla_Backend.Helpers.MlHelper;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ToxicityClassifier _toxicityClassifier;
        public ReviewController(IReviewService reviewService,ToxicityClassifier toxicityClassifier)
        {
            _reviewService = reviewService;
            _toxicityClassifier = toxicityClassifier;
        }

        [HttpGet("service-provider/{serviceProviderId}")]
        public async Task<IActionResult> GetReviewsByServiceProviderId(string serviceProviderId,string lan="en")
        {
            var reviews = await _reviewService.GetReviewsByServiceProviderIdAsync(serviceProviderId);
            return Ok(ResponseHelper.Success("GetReviewsSuccess",lan,reviews));
        }
        [HttpGet("ratings/{rating}/service-providers/{serviceProviderId}")]
        public async Task<IActionResult> GetReviewsByRatingForServiceProvider(int rating,string serviceProviderId, string lan = "en")
        {
            var reviews = await _reviewService.GetReviewsByRating(rating, serviceProviderId);
            return Ok(ResponseHelper.Success("GetReviewsSuccess", lan, reviews));
        }
        [HttpDelete("rating/{reviewid}")]
        public async Task<IActionResult>DeleteReview(int reviewid,string lan="en")
        {
            await _reviewService.DeleteReviewAsync(reviewid);
            return Ok(ResponseHelper.Success("ReviewDeletedSuccessfully", lan));
        }
        [HttpPost("AddReview")]

        public async Task<IActionResult> AddReview(AddReviewDto addReviewDto, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            await _reviewService.AddReviewAsync(addReviewDto);
            return Ok(ResponseHelper.Success("ReviewAddedSuccessfully", lan));
        }

        [HttpPut("UpdateReview")]
        public async Task<IActionResult> UpdateReview(UpdateReviewDto updateReviewDto, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            await _reviewService.UpdateReview(updateReviewDto); 
            return Ok(ResponseHelper.Success("ReviewUpdatedSuccessfully", lan));
        }
        [HttpGet("PredictToxicity")]
        public IActionResult PredictToxicity(string text)
        {
            var isBad = _toxicityClassifier.IsBadWord(text);

            return Ok(ResponseHelper.Success("ToxicityPredictionSuccess", "en", new
            {
                isToxic = isBad,
                textProcessed = text
            }));
        }


    }
}
