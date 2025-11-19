namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IResidentIdentityRepository : IGenericRepository<ResidentIdentity>
    {
        public Task<ResidentIdentity> GetByNationalIDAndGmail(string NationalID,string Gmail);
    }
}
