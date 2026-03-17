namespace Wasla_Backend.Repositories.Interfaces.technician
{
    public interface ITechnicianRepository:IGenericRepository<Technician>
    {
        public Task<Technician> GetByEmailAsync(string email);
        public Task<TechnicianProfileDto> GetProfileById(string id);
        public Task<bool>IsExistById(string id);
        public Task<List<TechnicianListDto>> GetTechniciansBySpecialty(TechnicianSpecialty? specialty, int pageNumber, int pageSize, string lan);
    }
}
