namespace Wasla_Backend.Repositories.Implementation
{
    public class ReservationRepository : GenericRepository<Reservations> , IReservationRepository
    {
        public ReservationRepository(Context context) : base(context)
        {
           
        }

        public async Task<PagedResult<Reservations>> GetRestaurantReservations(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .Where(r => r.restaurantId == dto.id)
                .Include(r => r.user)
                .OrderByDescending(r => r.id)
                .AsNoTracking();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }

        public async Task<PagedResult<Reservations>> GetResidentReservations(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .Where(r => r.userId == dto.id)
                .Include(r => r.restaurants)
                .OrderByDescending(r => r.id)
                .AsNoTracking()
                .AsQueryable();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }
    }
}
