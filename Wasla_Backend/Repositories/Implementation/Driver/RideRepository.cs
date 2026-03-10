
namespace Wasla_Backend.Repositories.Implementation.driver
{
    public class RideRepository : GenericRepository<Ride>, IRideRepository
    {
        public RideRepository(Context context) : base(context)
        {
        }
    }
}
