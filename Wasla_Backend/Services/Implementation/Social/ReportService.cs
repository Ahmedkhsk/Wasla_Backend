namespace Wasla_Backend.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly DateTimeHelper _dateTimeHelper;

        public ReportService(IReportRepository reportRepository,DateTimeHelper dateTimeHelper)
        {
            _reportRepository = reportRepository;
            _dateTimeHelper = dateTimeHelper;
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
        }

    }
}
