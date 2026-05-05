using Wasla_Backend.Repositories.Implementation;
namespace Wasla_Backend.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ISocialRepository _socialRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IUserRepository _userRepository;

        public ReportService(IReportRepository reportRepository, IDateTimeHelper dateTimeHelper,
            ISocialRepository socialRepository, IPostRepository postRepository,
            ICommentRepository commentRepository, IFileUrlBuilderService fileUrlBuilderService,IUserRepository userRepository)
        {
            _reportRepository = reportRepository;
            _dateTimeHelper = dateTimeHelper;
            _socialRepository = socialRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
            _userRepository = userRepository;
        }

        public async Task AddReport(AddReportDto dto)
        {
            var report = new Report
            {
                userId = dto.userId,
                reason = dto.reason,
                targetId = dto.targetId,
                targetType = dto.targetType,
                createdAt = _dateTimeHelper.Now
            };
            await _reportRepository.AddAsync(report);
            await _reportRepository.SaveChangesAsync();
        }

        public async Task DeleteReport(int reportId)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null)
                throw new NotFoundException(LocalizationKey.ReportNotFound);
            _reportRepository.Delete(report);
            await _reportRepository.SaveChangesAsync();
        }


        public async Task ChangeStatus(ToggleHideDto dto)
        {
            var target = await _socialRepository.GetSocialById(dto.id);
            if (target == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);

            target.isHidden = !target.isHidden;
            await _socialRepository.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(dto.reason))
            {
                var user = await _userRepository.GetUserByIdAsync(dto.adminId);
                if (user == null)
                    throw new NotFoundException(LocalizationKey.UserNotFound);

                var image = !string.IsNullOrEmpty(user.ProfilePhoto)
                    ? _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage)
                    : _fileUrlBuilderService.GetMediaUrl(defaultImage.defaultImageName, MediaType.userImage);

                var metadata = new Dictionary<string, string>
                {
                    { "ActorName", user.FullName ?? "User" },
                    { "Reason", dto.reason },
                };

                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x =>
                    x.sendNotification(
                        target.userId,
                        NotificationType.SocialHidden,
                        target.id.ToString(),
                        image,
                        "en",
                        metadata
                    )
                );
            }
        }
        public async Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams)
        {
            var reports = await _reportRepository.GetReports(paginationParams);

            var dataMapped = new List<GetReports>();

            foreach (var r in reports.Data)
            {
                if (r.targetType == ReactionTargetType.post)
                {
                    var post = await _postRepository.GetPostByIdIgnoreQF(r.targetId);
                    if (post != null)
                    {
                        r.images = post.files?
                            .Select(m => _fileUrlBuilderService.GetMediaUrl(m, MediaType.postFile))
                            .ToList();
                    }
                }
                else if (r.targetType == ReactionTargetType.comment)
                {
                    var comment = await _commentRepository.GetCommentByIdIgnoreQF(r.targetId);
                    if (comment != null)
                    {
                        r.image = _fileUrlBuilderService
                            .GetMediaUrl(comment.file, MediaType.postFile);
                    }
                }

                dataMapped.Add(r);
            }

            return new PagedResult<GetReports>
            {
                Data = dataMapped.ToList(),
                TotalCount = reports.TotalCount,
                PageSize = reports.PageSize,
                PageNumber = reports.PageNumber
            };
        }
    }
}
