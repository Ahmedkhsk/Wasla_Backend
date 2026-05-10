namespace Wasla_Backend.Services.Interfaces
{
    public interface IReservationService
    {
        Task AddReservatio(AddReservationDto dto);
        Task ChangeStatus(ChangeStatusOfReservationDto dto);
        Task UpdateReservation(UpdateReservationDto dto);
        Task<PagedResult<GetReservationsToRestaurantResponse>> GetRestaurantReservations(GetGeneralWithPaginationDto<string> dto);
        Task<PagedResult<GetReservationsToResidentReponse>> GetResidentReservations(GetGeneralWithPaginationDto<string> dto);
    }
}
