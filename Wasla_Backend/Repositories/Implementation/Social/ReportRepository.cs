namespace Wasla_Backend.Repositories.Implementation
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(Context context) : base(context)
        {
        }

        public async Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams)
        {
            var query = _dbSet
                .Include(r => r.target)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();

            if (paginationParams.flag != null)
            {
                query = query.Where(r => r.target.isHidden == paginationParams.flag);
            }

            var groupedQuery = query
                .GroupBy(r => new { r.targetId, r.targetType })
                .Select(group => new GetReports
                {
                    targetId = group.Key.targetId,
                    targetType = group.Key.targetType,

                    content = group.Select(x => x.target.content).FirstOrDefault(),
                    createdAt = group.Select(x => x.target.createdAt).FirstOrDefault(),
                    updatedAt = group.Select(x => x.target.updatedAt).FirstOrDefault(),

                    countReports = group.Count(),

                    reports = group.Select(r => new ReportResponse
                    {
                        id = r.id,
                        reason = r.reason,
                        userReportId = r.userId,
                        userNameReport = r.user.FullName,
                        userReportProfile = r.user.ProfilePhoto,
                        createdAt = r.createdAt
                    }).ToList(),
                })
                .OrderByDescending(x => x.countReports);

            return await groupedQuery.ToPagedResultAsync(
                paginationParams.PageNumber,
                paginationParams.PageSize
            );
        }
    }
}
