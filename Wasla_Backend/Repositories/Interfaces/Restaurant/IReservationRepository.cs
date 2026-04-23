namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservations>
    {
        public Task<PagedResult<Reservations>> GetRestaurantReservations(GetGeneralWithPaginationDto<string> dto);
        public Task<PagedResult<Reservations>> GetResidentReservations(GetGeneralWithPaginationDto<string> dto);
        public Task<int> CountReservations(string restaurantId);
    }
}
