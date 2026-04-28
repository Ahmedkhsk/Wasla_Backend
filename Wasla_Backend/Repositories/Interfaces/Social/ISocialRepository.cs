namespace Wasla_Backend.Repositories.Interfaces
{
    public interface ISocialRepository : IGenericRepository<Social>
    {
        Task<Social?> GetSocialById(int id);
    }
}
