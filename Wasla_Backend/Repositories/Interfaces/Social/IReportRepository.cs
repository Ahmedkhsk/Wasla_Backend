namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams);
    }
}
