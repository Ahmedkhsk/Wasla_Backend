namespace Wasla_Backend.Controllers.Social
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly IPostService _postService;

        public SocialController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> AddPost([FromForm] AddPostDto dto , string lan = "en")
        {
            await _postService.AddPost(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreatePost,lan));
        }

    }
}
