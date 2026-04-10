public class ResturantRepository : GenericRepository<Restaurant>, IRestaurantRepository
{
    public ResturantRepository(Context context) : base(context)
    {
    }

    
}
