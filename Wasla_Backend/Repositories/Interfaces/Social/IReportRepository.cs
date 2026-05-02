namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        public Task<Report?> GetReportByUserIdAndTargetId(string userId, int targetId);
        Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams);
    }
}
