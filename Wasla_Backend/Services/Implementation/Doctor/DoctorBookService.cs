using System.Numerics;
using Wasla_Backend.Models;

namespace Wasla_Backend.Services.Implementation
{
    public class DoctorBookService : IDoctorBookService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ServiceDay> _serviceDayRepository;
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IHubContext<BookingHub> _hub;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<DoctorBookService> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<DoctorBookService>();
        private static readonly SemaphoreSlim _bookingLock = new SemaphoreSlim(1, 1);

        public DoctorBookService(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IGenericRepository<ServiceDay> serviceDay,
            IDoctorServiceRepository doctorServiceRepository,
            IDoctorRepository doctorRepository,
            IResidentRepository residentRepository,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            IHubContext<BookingHub> hub,
            IMapper mapper,
            IPaymentService paymentService
        )
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _serviceDayRepository = serviceDay;
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _residentRepository = residentRepository;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _hub = hub;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task UpdateBookingStatus(int bookingId, BookingStatus status, bool isResident)
        {
            var booking = await _bookingRepository.GetByIdWithIncludeAsync(bookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);

            if (booking.bookingStatus == BookingStatus.completed)
                throw new BadRequestException(LocalizationKey.BookingStatusIsAlreadyCompleted);

            if (status == BookingStatus.all || !Enum.IsDefined(typeof(BookingStatus), status))
                throw new BadRequestException(LocalizationKey.InvalidBookingStatus);

            if (status == BookingStatus.canceled && booking.serviceDay != null)
            {
                booking.serviceDay.isBooking = false;
                var countOfBookings = await _bookingRepository.CountBookingBYUserAndServiceProvider(booking.ResidentId, booking.serviceProviderId);
                if (countOfBookings == 1 && booking.ServiceProviderType == ServiceProviderType.Doctor)
                {
                    var doctor = await _doctorRepository.GetByIdAsync(booking.serviceProviderId);
                    if (doctor != null && doctor.numberOfpatients > 0)
                    {
                        doctor.numberOfpatients -= 1;
                        _doctorRepository.Update(doctor);
                        await _doctorRepository.SaveChangesAsync();
                    }
                }
            }

            booking.bookingStatus = status;
            await _bookingRepository.SaveChangesAsync();

            var bookhubdata = new BookHubData
            {
                serviceId = booking.serviceDayId,
                residentId = booking.ResidentId,
                serviceProviderId = booking.serviceProviderId
            };
            await _hub.Clients.All.SendAsync("Bookingcanceled", bookhubdata);
            string photo;
            string TargetId;
            NotificationType type;
            if (isResident)
            {
                photo = _userRepository.GetUserPhoto(booking.ResidentId);
                TargetId = booking.serviceProviderId;
                type = NotificationType.residentCancelDoctorBooking;

            }
            else
            {
                photo = _userRepository.GetUserPhoto(booking.serviceProviderId);
                TargetId = booking.ResidentId;
                type = NotificationType.doctorCancelBookingScreen;

            }
            photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    TargetId,
    type,
    booking.Id.ToString(),
    photo,
    "en",
    null
));
            if (booking.isPaymentOnline)
            { 
                var entityTypeDto = new EntityTypeDto
                {
                    entityId = booking.Id,
                    entityType = EntityType.booking
                };
                await _paymentService.RefundPaymentAsync(entityTypeDto);
            }
        }

        public async Task UpdateBooking(UpdateBookingDto updateBookingDto)
        {
            var booking = await _bookingRepository.GetByIdAsync(updateBookingDto.BookingId);

            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);

            if (booking.bookingStatus == BookingStatus.completed)
                throw new BadRequestException(LocalizationKey.BookingStatusIsAlreadyCompleted);

            if (updateBookingDto.newDayOfWeek == WeekDayEnum.none ||
               string.IsNullOrWhiteSpace(updateBookingDto.newStart) ||
               string.IsNullOrWhiteSpace(updateBookingDto.newEnd))
                throw new BadRequestException(LocalizationKey.InvalidBookingUpdateDetails);

            booking = _mapper.Map(updateBookingDto, booking);
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            var bookhubdata = new BookHubData
            {
                serviceId = booking.serviceDayId,
                residentId = booking.ResidentId,
                serviceProviderId = booking.serviceProviderId
            };
            await _hub.Clients.All.SendAsync("BookingUpdated", bookhubdata);
            var photo = _userRepository.GetUserPhoto(booking.serviceProviderId);
            photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
      booking.ResidentId,
      NotificationType.doctorEditBookingScreen,
      booking.Id.ToString(),
     photo,
      "en",
      null
  ));
        }

        public async Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            return await _bookingRepository.GetBookingDetailsForUserAsync(userId, language);
        }

        public async Task<int> Book(BookServiceDto dto)
        {
            await _bookingLock.WaitAsync();
            try
            {
                var user = await _residentRepository.GetByIdAsync(dto.userId);
                if (user == null)
                    throw new NotFoundException(LocalizationKey.UserNotFound);

                var serviceProvider = await _userRepository.GetUserByIdAsync(dto.serviceProviderId);
                if (serviceProvider == null)
                    throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

                var hasanotherBooking = await _bookingRepository.HasBookingSameDay(dto.userId, dto.serviceProviderId, dto.bookingDate);
                if (hasanotherBooking)
                    throw new BadRequestException(LocalizationKey.UserHasAnotherBookingWithSameProviderOnThisDate);

                var serviceDay = await _serviceDayRepository.GetByIdAsync(dto.serviceDayId);
                if (serviceDay == null)
                    throw new NotFoundException(LocalizationKey.ServiceDayNotFound);

                if (serviceDay.isBooking)
                    throw new BadRequestException(LocalizationKey.ServiceAlreadyBooked);

                var savedImages = await _fileService.AddFilesAsync(
                    dto.images,
                    _fileUrlBuilderService.GetPath(MediaType.bookingImage)
                );

                var booking = new Booking
                {
                    ResidentId = dto.userId,
                    serviceProviderId = dto.serviceProviderId,
                    price = dto.price,
                    ServiceProviderType = dto.serviceProviderType,
                    bookingDate = dto.bookingDate,
                    bookingType = dto.bookingType,
                    images = savedImages,
                    serviceDayId = dto.serviceDayId,
                    isPaymentOnline = dto.isPaymentOnline
                };

                serviceDay.isBooking = true;
                _serviceDayRepository.Update(serviceDay);
                await _bookingRepository.AddAsync(booking);

                if (serviceProvider is Doctor doc)
                {
                    var hasExistingBooking = await _bookingRepository.GetByUserIdAndDoctorID(dto.userId, doc.Id);
                    if (!hasExistingBooking)
                    {
                        doc.numberOfpatients += 1;
                        _doctorRepository.Update(doc);
                    }
                }

                await _bookingRepository.SaveChangesAsync();
                await _doctorRepository.SaveChangesAsync();

                var bookhubdata = new BookHubData
                {
                    serviceId = dto.serviceDayId,
                    residentId = dto.userId,
                    serviceProviderId = dto.serviceProviderId
                };
                await _hub.Clients.All.SendAsync("ServiceDayBooked", bookhubdata);

                var endString = booking.newEnd ?? booking.serviceDay.end;

                if (TimeOnly.TryParse(endString, out var endTime))
                {
                    var endDateTime = booking.bookingDate.ToDateTime(endTime);
                    if (endTime <= TimeOnly.Parse(booking.serviceDay.start))
                        endDateTime = endDateTime.AddDays(1);

                    BackgroundJob.Schedule<HangfireFunctions>(
                        f => f.CompleteBookingAsync(booking.Id),
                        endDateTime
                    );
                }

                var metadata = new Dictionary<string, string>
{
    { "UserName", user.FullName ?? "User" },
    { "Date", dto.bookingDate.ToString() }
};

                var image = _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage);



                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                    serviceProvider.Id,
                    NotificationType.doctorBookingScreen,
                    booking.Id.ToString(),
                    image,
                    "en",
                    metadata
                ));
                if(booking.isPaymentOnline)
                {
                    Hangfire.BackgroundJob.Schedule(() => CheckPayment(booking.Id), TimeSpan.FromSeconds(110));

                }

                return booking.Id;

            }
            finally
            {
                _bookingLock.Release();
            }

        }
        public async Task CheckPayment(int bookingId)
        {
            var booking = await _bookingRepository.GetWithService(bookingId);
            if (booking == null)
                return;
            if (!booking.IsPaid)
            {
                booking.bookingStatus = BookingStatus.canceled;

                if (booking.serviceDay != null)
                {
                    booking.serviceDay.isBooking = false;
                    _serviceDayRepository.Update(booking.serviceDay);
                }
                _bookingRepository.Update(booking);
                await _bookingRepository.SaveChangesAsync();
                var bookhubdata = new BookHubData
                {
                    serviceId = booking.serviceDayId,
                    residentId = booking.ResidentId,
                    serviceProviderId = booking.serviceProviderId
                };
                await _hub.Clients.All.SendAsync("Bookingcanceled", bookhubdata);
            }
        }
    }
}