namespace Wasla_Backend.Repositories.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        public Task<Cart?> GetCartAsync(string residentId, string restaurantId);

    }
}
