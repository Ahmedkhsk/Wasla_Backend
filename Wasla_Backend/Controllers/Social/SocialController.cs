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

        [HttpGet("Posts")]
        public async Task<IActionResult> GetPostsGeneral(int pageNumber = 1, int pageSize = 10, string lan = "en")
        {
            var posts = await _postService.GetPostsGeneral(pageNumber, pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, lan, posts));
        }
        [HttpPost("CheckReact")]
        public async Task<IActionResult> CheckReact(ToggleReactionDto dto, string lan = "en")
        {
            var hasReacted = await _reactionService.CheckReact(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCheckReaction, lan, hasReacted));
        }
    }
}
