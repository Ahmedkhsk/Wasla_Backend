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
                .OrderByDescending(r => r.reservationDate)
                .AsNoTracking();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }

        public async Task<PagedResult<Reservations>> GetResidentReservations(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .Where(r => r.userId == dto.id)
                .Include(r => r.restaurants)
                .OrderByDescending(r => r.reservationDate)
                .AsNoTracking()
                .AsQueryable();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }

        public async Task<int> CountReservations(string restaurantId)
        {
            return await _dbSet.Where(o => o.restaurantId == restaurantId&& o.status == Status.Completed).CountAsync();
        }

        public async Task<Reservations> GetWithResidentAndRestaurant(int reservationId)
        {
            return await _dbSet.Where(r => r.id == reservationId)
                .Include(r => r.user)
                .Include(r => r.restaurants)
                .FirstOrDefaultAsync();
        }
    }
}
