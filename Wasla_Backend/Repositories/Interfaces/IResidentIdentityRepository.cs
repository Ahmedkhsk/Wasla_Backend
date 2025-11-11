namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IResidentIdentityRepository : IGenericRepository<ResidentIdentity>
    {
        public Task<ResidentIdentity> GetByNationalID(string NationalID);
    }
}
