namespace Wasla_Backend.Controllers.Social
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IReactionService _reactionService;
        private readonly ICommentService _commentService;

        public SocialController(IPostService postService, IReactionService reactionService,ICommentService commentService)
        {
            _postService = postService;
            _reactionService = reactionService;
            _commentService = commentService;
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

        [HttpGet("Posts")]
        public async Task<IActionResult> GetPostsGeneral(string currentUserId, int pageNumber = 1, int pageSize = 10, string lan = "en")
        {
            var posts = await _postService.GetPostsGeneral(currentUserId,pageNumber, pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, lan, posts));
        }

        [HttpGet("Posts/{userId}")]
        public async Task<IActionResult> GetPostsByUser(string userId, string currentUser, int pageNumber = 1, int pageSize = 10, string lan = "en")
        {
            var posts = await _postService.GetPostsByUserId(userId, currentUser, pageNumber, pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, lan, posts));
        }

        [HttpPost("Comment")]
        public async Task<IActionResult> AddComment(AddCommentDto dto,string lan)
        {
            await _commentService.AddComment(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.FailedToCreateComment, lan));
        }

        [HttpPut("Commnet")]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto dto, string lan)
        {
            await _commentService.UpdateComment(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdateComment, lan));
        }

        [HttpDelete("Comment")]
        public async Task<IActionResult> DeleteComment(int commentId, string lan)
        {
            await _commentService.DeleteComment(commentId);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeleteComment,lan));
        }

        [HttpPost("ToggleReaction")]
        public async Task<IActionResult> ToggleReaction(ToggleReactionDto dto, string lan = "en")
        {
            await _reactionService.ToggleReaction(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToToggleReaction, lan));
        }

        [HttpPost("CheckReact")]
        public async Task<IActionResult> CheckReact(ToggleReactionDto dto, string lan = "en")
        {
            var hasReacted = await _reactionService.CheckReact(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCheckReaction, lan, hasReacted));
        }

    }
}
