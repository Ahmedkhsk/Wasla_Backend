namespace Wasla_Backend.Controllers.Social
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IReactionService _reactionService;
        private readonly ICommentService _commentService;
        private readonly IReportService _reportService;

        public SocialController(IPostService postService, IReactionService reactionService,
                                ICommentService commentService, IReportService reportService)
        {
            _postService = postService;
            _reactionService = reactionService;
            _commentService = commentService;
            _reportService = reportService;
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
        public async Task<IActionResult> GetPostsGeneral(string currentUserId, [FromQuery] PaginationParams paginationParams, string lan = "en")
        {
            var posts = await _postService.GetPostsGeneral(currentUserId, paginationParams);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, lan, posts));
        }

        [HttpGet("Posts/{userId}")]
        public async Task<IActionResult> GetPostsByUser(string userId, string currentUser, int pageNumber = 1, int pageSize = 10, string lan = "en")
        {
            var posts = await _postService.GetPostsByUserId(userId, currentUser, pageNumber, pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, lan, posts));
        }

        [HttpGet("Posts/ReactionType")]
        public async Task<IActionResult> GetPostsByUsingReactionType([FromQuery] GetPostsByUsingReactionTypeDto dto)
        {
            var posts = await _postService.GetPostsByUsingReactionType(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, dto.lan, posts));
        }

        [HttpPost("Comment")]
        public async Task<IActionResult> AddComment(AddCommentDto dto, string lan = "en")
        {
            await _commentService.AddComment(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreateComment, lan));
        }

        [HttpPut("Commnet")]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto dto, string lan = "en")
        {
            await _commentService.UpdateComment(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdateComment, lan));
        }

        [HttpDelete("Comment")]
        public async Task<IActionResult> DeleteComment(int commentId, string lan = "en")
        {
            await _commentService.DeleteComment(commentId);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeleteComment, lan));
        }

        [HttpGet("Comments")]
        public async Task<IActionResult> GetCommentsByPostId([FromQuery] GetCommentDto dto)
        {
            var comments = await _commentService.GetCommentsResponsesByPostId(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetComments, dto.lan, comments));
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

        [HttpGet("InformationProfile")]
        public async Task<IActionResult> InformationProfile(string userId, string lan = "en")
        {
            var informationProfile = await _postService.InformationProfileResponse(userId);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetInformationProfile, lan, informationProfile));
        }

        [HttpPost("Report")]
        public async Task<IActionResult> Report(AddReportDto dto , [FromQuery] LanDto lanDto)
        {
            await _reportService.AddReport(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToReport, lanDto.lan));
        }

        [HttpPut("Toggle_Hide")]
        public async Task<IActionResult> HidePostOrComment([FromQuery] GetGeneralDto<int> dto)
        {
            await _reportService.ChangeStatus(dto.id);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToToggleContent, dto.lan));
        }

        [HttpGet("Reports")]
        public async Task<IActionResult> GetReports([FromQuery] PaginationParams dto)
        {
            var reports = await _reportService.GetReports(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetReports, dto.lan, reports));
        }
    }
}
