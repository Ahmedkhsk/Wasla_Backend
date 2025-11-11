namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IResidentRepository : IGenericRepository<Resident>
    {
        Task<Resident> GetByEmail(string email);
    }
}
