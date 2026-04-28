namespace Wasla_Backend.Services.Interfaces
{
    public interface IReportService
    {
        Task AddReport(AddReportDto dto);
        Task ChangeStatus(int taegetId);
        Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams);
    }
}
