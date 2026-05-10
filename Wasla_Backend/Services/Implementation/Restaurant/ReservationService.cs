using Wasla_Backend.Models;

namespace Wasla_Backend.Services.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationsRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public ReservationService(
            IReservationRepository reservationsRepository, IRestaurantRepository restaurantRepository,
            IResidentRepository residentRepository,IFileUrlBuilderService fileUrlBuilderService,
            IMapper mapper, IFileService fileService , IDateTimeHelper dateTimeHelper,
            IUserAuthorizationService userAuthorizationService)
        {
            _reservationsRepository = reservationsRepository;
            _restaurantRepository = restaurantRepository;
            _residentRepository = residentRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
            _mapper = mapper;
            _fileService = fileService;
            _dateTimeHelper = dateTimeHelper;
            _userAuthorizationService = userAuthorizationService;
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

            var jobId = BackgroundJob.Schedule<HangfireFunctions>(
               x => x.CheckReservationStatus(reservation.id),
               _dateTimeHelper.CalculateDelay(reservation.reservationDate, reservation.reservationTime)
            );

            reservation.jobId = jobId;

            reservation.status = Status.Pending;

            await _reservationsRepository.AddAsync(reservation);
            await _reservationsRepository.SaveChangesAsync();


            var metadata = new Dictionary<string, string>
            {   
                { "UserName", resident.FullName ?? "User" },
                { "Date", dto.reservationDate.ToString() },
                { "Persons", dto.numberOfPersons.ToString() }
            };

            var UserImage = _fileUrlBuilderService.GetMediaUrl(
                resident.ProfilePhoto,
                MediaType.userImage
            );
           
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                reservation.restaurantId,
                NotificationType.restaurantNewReservation,
                reservation.id.ToString(),
                UserImage,
                "en",
                metadata
           ));
        }

        public async Task ChangeStatus(ChangeStatusOfReservationDto dto)
        {
            var reservation = await _reservationsRepository.GetWithResidentAndRestaurant(dto.reservationId);
            if (reservation == null)
                throw new NotFoundException(LocalizationKey.ReservationNotFound);

            if (dto.isResident)
                await _userAuthorizationService.CheckOwnershipByIdAsync(reservation.userId);
            else
                await _userAuthorizationService.CheckOwnershipByIdAsync(reservation.restaurantId);

            if (dto.isResident && dto.status != Status.Canceled && reservation.status != Status.Pending) 
                throw new BadRequestException(LocalizationKey.CannotCancelReservation);

            reservation.status = dto.status;
        
            if (dto.status == Status.Accepted)
            {
                var QrData = new
                {
                    reservationId = reservation.id,
                    restaurantName = reservation.restaurants.BusinessName,
                    residentName = reservation.user.FullName,
                    numberOfPersons = reservation.numberOfPersons,
                    reservationDate = reservation.reservationDate,
                    reservationTime = reservation.reservationTime,
                    residentImage = _fileUrlBuilderService.GetMediaUrl(
                      reservation.user.ProfilePhoto,
                      MediaType.userImage)
                }; 
               var QrCode= QRHelper.GenerateQRFile(QrData, fileName: $"Reservation_{reservation.id}.png");
               reservation.QRCode= await _fileService.AddFileAsync(QrCode, _fileUrlBuilderService.GetPath(MediaType.qrCode));
                _reservationsRepository.Update(reservation);
                await _reservationsRepository.SaveChangesAsync();
                var RestaurantImage = _fileUrlBuilderService.GetMediaUrl(
                    reservation.restaurants.ProfilePhoto,
                    MediaType.userImage
                );
                var QrPath = _fileUrlBuilderService.GetMediaUrl(
                    reservation.QRCode,
                    MediaType.qrCode
                );
                var metadata = new Dictionary<string, string>
{
                    { "RestaurantName", reservation.restaurants.BusinessName ?? "Restaurant" },
                    { "Date", reservation.reservationDate.ToString() }
                };
                Hangfire.BackgroundJob.Enqueue<NotificationFunction>( x => x.sendNotification(
                reservation.userId,
                NotificationType.restaurantReservationAccepted,
                QrPath,
                RestaurantImage,
                "en",
                metadata
            ));
            }
            _reservationsRepository.Update(reservation);
            await _reservationsRepository.SaveChangesAsync();
        }

        public async Task UpdateReservation(UpdateReservationDto dto)
        {
            var reservation = await _reservationsRepository.GetWithResidentAndRestaurant(dto.reservationId);
            if (reservation == null) 
                throw new NotFoundException(LocalizationKey.ReservationNotFound);

            await _userAuthorizationService.CheckOwnershipByIdAsync(reservation.userId);

            if (reservation.status != Status.Pending)
                throw new BadRequestException(LocalizationKey.CannotEditReservation);

            reservation.numberOfPersons = dto.numberOfPersons;
            reservation.reservationTime = dto.reservationTime;
            reservation.reservationDate = dto.reservationDate;

            if (!string.IsNullOrEmpty(reservation.jobId))
            {
                BackgroundJob.Delete(reservation.jobId);
            }

            var jobId = BackgroundJob.Schedule<HangfireFunctions>(
               x => x.CheckReservationStatus(reservation.id),
               _dateTimeHelper.CalculateDelay(reservation.reservationDate, reservation.reservationTime)
            );

            reservation.jobId = jobId;

            _reservationsRepository.Update(reservation);
            await _reservationsRepository.SaveChangesAsync();

            var userImage = _fileUrlBuilderService.GetMediaUrl(
                reservation.user!.ProfilePhoto,
                MediaType.userImage
            );

            var metadata = new Dictionary<string, string>
            {
                { "UserName", reservation.user.FullName ?? "User" },
                { "Date", reservation.reservationDate.ToString("yyyy-MM-dd") },
                { "Time", reservation.reservationTime.ToString() },
                { "Persons", reservation.numberOfPersons.ToString() }
            };

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    reservation.restaurantId,
                    NotificationType.restaurantReservationUpdated,
                    reservation.id.ToString(),
                    userImage,
                    "en",
                    metadata
                )
            );
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
                mapped.QRCode = _fileUrlBuilderService.GetMediaUrl(
                    r.QRCode,
                    MediaType.qrCode
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
