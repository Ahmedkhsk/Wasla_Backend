namespace Wasla_Backend.Controllers.Social
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> AddPost([FromForm] AddPostDto dto, [FromQuery] LanDto lanDto)
        {
            await _postService.AddPost(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreatePost, lanDto.lan));
        }

        [HttpPut("Post")]
        public async Task<IActionResult> UpdatePost([FromForm] UpdatePostDto dto, [FromQuery] LanDto lanDto)
        {
            await _postService.UpdatePost(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdatePost, lanDto.lan));
        }

        [HttpDelete("Post")]
        public async Task<IActionResult> DeletePost(int postId, [FromQuery] LanDto lanDto)
        {
            await _postService.DeletePost(postId);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeletePost, lanDto.lan));
        }

        [HttpGet("Posts")]
        public async Task<IActionResult> GetPostsGeneral(string currentUserId, [FromQuery] PaginationParams paginationParams)
        {
            var posts = await _postService.GetPostsGeneral(currentUserId, paginationParams);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, paginationParams.lan, posts));
        }

        [HttpGet("Posts/{userId}")]
        public async Task<IActionResult> GetPostsByUser(string userId, string currentUser,
                                                        int pageNumber = 1, int pageSize = 10,
                                                        [FromQuery] LanDto lanDto = null!)
        {
            var posts = await _postService.GetPostsByUserId(userId, currentUser, pageNumber, pageSize);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, lanDto.lan, posts));
        }

        [HttpGet("Posts/ReactionType")]
        public async Task<IActionResult> GetPostsByUsingReactionType([FromQuery] GetPostsByUsingReactionTypeDto dto)
        {
            var posts = await _postService.GetPostsByUsingReactionType(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetPosts, dto.lan, posts));
        }

        [HttpPost("Comment")]
        public async Task<IActionResult> AddComment(AddCommentDto dto, [FromQuery] LanDto lanDto)
        {
            await _commentService.AddComment(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreateComment, lanDto.lan));
        }

        [HttpPut("Comment")]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto dto, [FromQuery] LanDto lanDto)
        {
            await _commentService.UpdateComment(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdateComment, lanDto.lan));
        }

        [HttpDelete("Comment")]
        public async Task<IActionResult> DeleteComment(int commentId, [FromQuery] LanDto lanDto)
        {
            await _commentService.DeleteComment(commentId);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeleteComment, lanDto.lan));
        }

        [HttpGet("Comments")]
        public async Task<IActionResult> GetCommentsByPostId([FromQuery] GetCommentDto dto)
        {
            var comments = await _commentService.GetCommentsResponsesByPostId(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetComments, dto.lan, comments));
        }

        [HttpPost("ToggleReaction")]
        public async Task<IActionResult> ToggleReaction(ToggleReactionDto dto, [FromQuery] LanDto lanDto)
        {
            await _reactionService.ToggleReaction(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToToggleReaction, lanDto.lan));
        }

        [HttpPost("CheckReact")]
        public async Task<IActionResult> CheckReact(ToggleReactionDto dto, [FromQuery] LanDto lanDto)
        {
            var hasReacted = await _reactionService.CheckReact(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCheckReaction, lanDto.lan, hasReacted));
        }

        [HttpGet("InformationProfile")]
        public async Task<IActionResult> InformationProfile(string userId, [FromQuery] LanDto lanDto)
        {
            var informationProfile = await _postService.InformationProfileResponse(userId);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetInformationProfile,
                                             lanDto.lan,
                                             informationProfile));
        }

        [HttpPost("Report")]
        public async Task<IActionResult> Report(AddReportDto dto, [FromQuery] LanDto lanDto)
        {
            await _reportService.AddReport(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToReport, lanDto.lan));
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpPut("Toggle_Hide")]
        public async Task<IActionResult> HidePostOrComment([FromQuery] ToggleHideDto dto)
        {
            await _reportService.ChangeStatus(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToToggleContent, dto.lan));
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpDelete("Report")]
        public async Task<IActionResult> DeleteReport([FromQuery] GetGeneralDto<int> dto)
        {
            await _reportService.DeleteReport(dto.id);

            return Ok(ResponseHelper.Success(LocalizationKey.ReportDeleted, dto.lan));
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpGet("Reports")]
        public async Task<IActionResult> GetReports([FromQuery] PaginationParams dto)
        {
            var reports = await _reportService.GetReports(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetReports, dto.lan, reports));
        }
    }
}