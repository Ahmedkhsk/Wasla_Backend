namespace Wasla_Backend.Repositories.Implementation
{
    public class UserEventRepository : GenericRepository<UserEvent>, IUserEventRepository
    {
        public UserEventRepository(Context context) : base(context)
        {
        }
    }
}
