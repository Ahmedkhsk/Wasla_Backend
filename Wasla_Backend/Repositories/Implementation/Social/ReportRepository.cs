namespace Wasla_Backend.Repositories.Implementation
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(Context context) : base(context)
        {
        }


    }
}
