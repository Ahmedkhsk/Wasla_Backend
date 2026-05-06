namespace Wasla_Backend.Repositories.Interfaces.Authentication
{
    public interface IAdminRepository : IGenericRepository<Admin>   
    {
        public Task <List<AdminResponseDto>> GetAllAdminsAsync();
    }
}
