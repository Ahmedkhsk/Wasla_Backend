namespace Wasla_Backend.Repositories.Interfaces.Gyms
{
    public interface IGymRepository : IGenericRepository<Gym>
    {
        public Task<List<AllGymsDataDto>> AllGyms();
        public Task<GymProfileDto> GymProfile(string id);
        public Task<Gym> GetByGmailAsync(string gmail);
    }
}
