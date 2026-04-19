namespace Wasla_Backend.Services.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationsRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IMapper _mapper;

        public ReservationService(
            IReservationRepository reservationsRepository, IRestaurantRepository restaurantRepository,
            IResidentRepository residentRepository,IFileUrlBuilderService fileUrlBuilderService,
            IMapper mapper)
        {
            _reservationsRepository = reservationsRepository;
            _restaurantRepository = restaurantRepository;
            _residentRepository = residentRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
            _mapper = mapper;
        }

        public async Task AddReservatio(AddReservationDto dto)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(dto.restaurantId);
            if (restaurant == null)
                throw new NotFoundException(LocalizationKey.RestaurantNotFound);

            var resident = await _residentRepository.GetByIdAsync(dto.userId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

       
            var reservation = new Reservations
            {
                userId = dto.userId,
                restaurantId = dto.restaurantId,
                numberOfPersons = dto.numberOfPersons,
                reservationDate = dto.reservationDate,
                reservationTime = dto.reservationTime
            };
            
            reservation.status = Status.Pending;

            await _reservationsRepository.AddAsync(reservation);
            await _reservationsRepository.SaveChangesAsync();
        }

        public async Task ChangeStatus(int reservationId , Status status)
        {
            var reservation = await _reservationsRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new NotFoundException(LocalizationKey.ReservationNotFound);
            reservation.status = status;
            _reservationsRepository.Update(reservation);
            await _reservationsRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<GetReservationsToRestaurantResponse>> GetRestaurantReservations(GetGeneralWithPaginationDto<string> dto)
        {
            var result = await _reservationsRepository.GetRestaurantReservations(dto);

            var mappedItems = result.Data.Select(r =>
            {
                var mapped = _mapper.Map<GetReservationsToRestaurantResponse>(r);

                mapped.profile = _fileUrlBuilderService.GetMediaUrl(
                    r.user.ProfilePhoto,
                    MediaType.userImage
                );

                return mapped;
            }).ToList();

            return new PagedResult<GetReservationsToRestaurantResponse>
            {
                Data = mappedItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<PagedResult<GetReservationsToResidentReponse>> GetResidentReservations(GetGeneralWithPaginationDto<string> dto)
        {
            var result = await _reservationsRepository.GetResidentReservations(dto);

            var mappedItems = result.Data.Select(r =>
            {
                var mapped = _mapper.Map<GetReservationsToResidentReponse>(r);

                mapped.restaurantProfile = _fileUrlBuilderService.GetMediaUrl(
                    r.restaurants.ProfilePhoto,
                    MediaType.userImage
                );

                return mapped;
            }).ToList();

            return new PagedResult<GetReservationsToResidentReponse>
            {
                Data = mappedItems,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }
    }
}
