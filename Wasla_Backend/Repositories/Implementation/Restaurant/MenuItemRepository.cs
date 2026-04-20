namespace Wasla_Backend.Repositories.Implementation
{
    public class MenuItemRepository : GenericRepository<MenuItem> , IMenuItemRepository
    {
        public MenuItemRepository(Context context) : base(context)
        {
        }



    }
}
