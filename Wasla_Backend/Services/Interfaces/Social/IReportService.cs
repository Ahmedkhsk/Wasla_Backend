namespace Wasla_Backend.Services.Interfaces
{
    public interface IReportService
    {
        Task AddReport(AddReportDto dto);
        Task ChangeStatus(ToggleHideDto dto);
        Task<PagedResult<GetReports>> GetReports(PaginationParams paginationParams);
    }
}
