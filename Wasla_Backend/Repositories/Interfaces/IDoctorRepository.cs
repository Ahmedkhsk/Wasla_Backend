namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        public Task<Doctor> GetByEmail(string email);
        public Task<Doctor> GetById(string id);
        public Task<IEnumerable<Doctor>> GetBySpecialist(int specialistId);
        public  Task<string?> GetDoctorSpecializationName(string doctorId, string language);
            
            }
}
