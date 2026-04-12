namespace Wasla_Backend.Services.Interfaces
{
    public interface IReservationService
    {
        Task AddReservatio(AddReservationDto dto);

        Task ChangeStatus(int reservationId, Status status);

        Task<PagedResult<GetReservationsToRestaurantResponse>> GetRestaurantReservations(GetGeneralWithPaginationDto<string> dto);
    }
}
