using Wasla_Backend.Repositories.Implementation;

namespace Wasla_Backend.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly ISocialRepository _socialRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public ReportService(IReportRepository reportRepository, DateTimeHelper dateTimeHelper,
            ISocialRepository socialRepository, IPostRepository postRepository,
            ICommentRepository commentRepository, IFileUrlBuilderService fileUrlBuilderService)
        {
            _reportRepository = reportRepository;
            _dateTimeHelper = dateTimeHelper;
            _socialRepository = socialRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
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

        public async Task ChangeStatus(int taegetId)
        {
            var target = await _socialRepository.GetSocialById(taegetId);
            if (target == null)
                throw new NotFoundException(LocalizationKey.PostNotFound);
            
            target.isHidden = !target.isHidden;
            await _reportRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams)
        {
            var reports = await _reportRepository.GetReports(paginationParams);

            var dataMapped = await Task.WhenAll(
                reports.Data.Select(async r =>
                {
                    if (r.targetType == ReactionTargetType.post)
                    {
                        var post = await _postRepository.GetByIdAsync(r.targetId);
                        if (post != null)
                        {
                            r.images = post.files?
                                .Select(m => _fileUrlBuilderService.GetMediaUrl(m, MediaType.postFile))
                                .ToList();
                        }
                    }
                    else if (r.targetType == ReactionTargetType.comment)
                    {
                        var comment = await _commentRepository.GetByIdAsync(r.targetId);
                        if (comment != null)
                        {
                            r.image = _fileUrlBuilderService
                                .GetMediaUrl(comment.file, MediaType.postFile);
                        }
                    }

                    return r;
                })
            );

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
