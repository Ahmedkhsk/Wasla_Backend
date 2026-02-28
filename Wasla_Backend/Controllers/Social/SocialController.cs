namespace Wasla_Backend.Controllers.Social
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IReactionService _reactionService;

        public SocialController(IPostService postService, IReactionService reactionService)
        {
            _postService = postService;
            _reactionService = reactionService;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> AddPost([FromForm] AddPostDto dto, string lan = "en")
        {
            await _postService.AddPost(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreatePost, lan));
        }

        [HttpPut("Post")]
        public async Task<IActionResult> UpdatePost([FromForm] UpdatePostDto dto, string lan = "en")
        {
            await _postService.UpdatePost(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdatePost, lan));

        }

        [HttpDelete("Post")]
        public async Task<IActionResult> DeletePost(int postId, string lan = "en")
        {
            await _postService.DeletePost(postId);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeletePost, lan));
        }

        [HttpPost("ToggleReaction")]
        public async Task<IActionResult> ToggleReaction(ToggleReactionDto dto, string lan = "en")
        {
            await _reactionService.ToggleReaction(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToToggleReaction, lan));
        }


    }
}
