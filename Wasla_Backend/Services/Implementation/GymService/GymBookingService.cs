using Microsoft.EntityFrameworkCore;

namespace Wasla_Backend.Services.Implementation.GymService
{
    public class GymBookingService : IGymBookingService
    {
        private readonly IGymBookingRepository _gymBookingRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IGymRepository _gymRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IHubContext<BookingHub> _hub;
        private readonly Context _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentService _paymentService;

        public GymBookingService(
            IGymBookingRepository gymBookingRepository,
            IPackageRepository packageRepository,
            IGymRepository gymRepository,
            IResidentRepository residentRepository,
            IMapper mapper,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            IDateTimeHelper dateTimeHelper,
            IHubContext<BookingHub> hub,
            Context dbContext,
            IUserRepository userRepository,
            IPaymentService paymentService
        )
        {
            _gymBookingRepository = gymBookingRepository;
            _packageRepository = packageRepository;
            _gymRepository = gymRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _dateTimeHelper = dateTimeHelper;
            _hub = hub;
            _dbContext = dbContext;
            _userRepository = userRepository;
            _paymentService = paymentService;
        }

        public async Task<BookResponse> Book(GymBookDto gymBookDto, string lan)
        {
            var gym = await _gymRepository.GetByIdAsync(gymBookDto.gymId);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            var gymPhotoUrl = _fileUrlBuilderService.GetMediaUrl(gym.ProfilePhoto, MediaType.gymImage);

            var resident = await _residentRepository.GetByIdAsync(gymBookDto.residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

            var service = await _packageRepository.GetByIdAsync(gymBookDto.serviceId);
            if (service == null)
                throw new NotFoundException(LocalizationKey.PackageNotFound);

            var IsexistingBooking = await _gymBookingRepository.IsBookingExist(gymBookDto.residentId, gymBookDto.serviceId);
            if (IsexistingBooking)
                throw new BadRequestException(LocalizationKey.PackageAlreadyBooked);

            var gymBooking = _mapper.Map<GymBooking>(gymBookDto);
            gymBooking.Date = _dateTimeHelper.Now;
            gymBooking.isPaymentOnline =gymBookDto.isPaymentOnline;

            int durationInMonths;

            if (service.type == GymServiceType.Package)
            {
                durationInMonths = service.DurationInMonths;
                gymBooking.price =(double) service.Price;
            }
            else
            {
                durationInMonths = service.DurationInMonths;
                var discountValue = service.Price * (service.Precentage / 100m);
                gymBooking.price = (double)(service.Price - discountValue);
            }

            gymBooking.ServiceProviderType = ServiceProviderType.Gym;
            gymBooking.Service = service;

         

            var QrData = new QrCodeDto
            {
                bookingId = gymBooking.Id,
                residentName = resident.FullName,
                residentPhoto = resident.ProfilePhoto,
                gymName = gym.BusinessName,
                serviceName = service.Name.GetText(lan),
                bookingTime = gymBooking.Date,
                expiryDate = gymBooking.Date.AddMonths(durationInMonths),
                bookingStatus = gymBooking.BookingStatus
            };

            var qrcode = QRHelper.GenerateQRFile(QrData);
            
            var filePath = await _fileService.AddFileAsync(qrcode, _fileUrlBuilderService.GetPath(MediaType.qrCode));
            gymBooking.QrCode = filePath;
            await _gymBookingRepository.AddAsync(gymBooking);
            await _gymBookingRepository.SaveChangesAsync();

            if (gymBooking.isPaymentOnline)
            {
                Hangfire.BackgroundJob.Schedule(
                () => CheckPayment(filePath, gymBooking.Id, gym.BusinessName, gymPhotoUrl, lan),
                TimeSpan.FromSeconds(100)
            );
            }

            var expiryDate = gymBooking.Date.AddMonths(durationInMonths);
            var delay = expiryDate - _dateTimeHelper.Now;
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.Zero;

            BackgroundJob.Schedule<GymBookingService>(
                x => x.ExpireBooking(gymBooking.Id),
                delay
            );
            var metadata = new Dictionary<string, string>
{
    { "UserName", resident.FullName ?? "User" },
    { "PackageName", service.Name.English    }
};
            var image = _fileUrlBuilderService.GetMediaUrl(resident.ProfilePhoto, MediaType.userImage);

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                gym.Id,
                NotificationType.gymPackageBooked,
                gymBooking.Id.ToString(),
                image,
                lan,
                metadata
            ));

            return new BookResponse
            {
                serviceId = gymBookDto.serviceId,
                serviceProviderId = gymBookDto.gymId,
                residentId = gymBookDto.residentId,
                bookingId = gymBooking.Id
            };
        }

        public async Task ExpireBooking(int gymBookingId)
        {
            var booking = await _gymBookingRepository.GetByIdAsync(gymBookingId);
            if (booking == null) return;

            booking.BookingStatus = GymBookingStatus.Completed;
            await _gymBookingRepository.SaveChangesAsync();
            var photo = _userRepository.GetUserPhoto(booking.GymId);
            photo = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    booking.ResidentId,
    NotificationType.gymPackageExpired,
    booking.ServiceId.ToString(),
    photo,
    "en",
    null
));
        }

        public async Task CheckPayment(string qrPath, int bookingId, string gymName, string gymPhotoUrl, string lan)
        {
            var booking = await _gymBookingRepository.GetByIdAsync(bookingId);
            if (booking == null) return;

            var metadata = new Dictionary<string, string>
            {
                { "GymName", gymName ?? string.Empty }
            };

            if (!booking.IsPaid)
            {
                booking.BookingStatus = GymBookingStatus.Cancelled;
                await _gymBookingRepository.SaveChangesAsync();

                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                    x => x.sendNotification(
                        booking.ResidentId,
                        NotificationType.gymPaymentFailed,
                        booking.Id.ToString(),
                        gymPhotoUrl,
                        lan,
                        metadata
                    ));
            }
            else
            {
                booking.BookingStatus = GymBookingStatus.Active;
                await _gymBookingRepository.SaveChangesAsync();

                var qrUrl = _fileUrlBuilderService.GetMediaUrl(qrPath, MediaType.qrCode);

                await _hub.Clients.User(booking.ResidentId).SendAsync("PaymentConfirmed", qrUrl);

                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                    x => x.sendNotification(
                        booking.ResidentId,
                        NotificationType.gymPaymentSuccess,
                        qrUrl,
                        gymPhotoUrl,
                        lan,
                        metadata
                    ));
            }
        }

        public async Task<BookHubData> Cancel(int bookingId, bool isResident)
        {
            var booking = await _gymBookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new NotFoundException(LocalizationKey.BookingNotFound);
            await _dbContext.Entry(booking).Reference(x => x.Resident).LoadAsync();
            await _dbContext.Entry(booking).Reference(x => x.Service).LoadAsync();
            await _dbContext.Entry(booking).Reference(x => x.Gym).LoadAsync();

            booking.BookingStatus = GymBookingStatus.Cancelled;
            _gymBookingRepository.Update(booking);
            await _gymBookingRepository.SaveChangesAsync();
            var userName =  isResident ? booking.Resident.FullName : booking.Gym.BusinessName;
            var packageName = booking.Service?.Name.English ?? "Package";
           var photo= isResident ? booking.Resident.ProfilePhoto : booking.Gym.ProfilePhoto;
            var photourl = _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
            var targetId = isResident ? booking.GymId : booking.ResidentId;
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
   targetId,
    NotificationType.gymBookingCancelled,
    booking.Id.ToString(),
    photourl,
    "en",
    new Dictionary<string, string>
    {
        { "UserName", userName },
        { "PackageName", packageName }
    }
));

            return new BookHubData
            {
                serviceId = booking.ServiceId,
                serviceProviderId = booking.GymId,
                residentId = booking.ResidentId
            };
            if(booking.isPaymentOnline)
            {
                var entityTypeDto = new EntityTypeDto
                {
                    entityId = booking.Id,
                    entityType = EntityType.booking
                };
                await _paymentService.RefundPaymentAsync(entityTypeDto);
            }
        }

        public async Task<List<BookingOfGym>> PackageBookingOFGym(string gymId)
        {
            var gym = await _gymRepository.GetByIdAsync(gymId);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            return await _gymBookingRepository.PackagebookingOfGym(gymId);
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status)
        {
            var gym = await _gymRepository.GetByIdAsync(gymId);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            return await _gymBookingRepository.PackagebookingOfGymAndStatus(gymId, status);
        }

        public async Task<List<BookingOfUser>> PackagebookingOfResident(string residentId)
        {
            var resident = await _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

            var data= await _gymBookingRepository.PackagebookingOfResident(residentId);
            data.ForEach(d => d.QrCode = _fileUrlBuilderService.GetMediaUrl(d.QrCode, MediaType.qrCode));
            return data;
        }

        public async Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status)
        {
            var resident = await _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

            var data= await _gymBookingRepository.PackagebookingOfResidentAndStatus(residentId, status);
            data.ForEach(d => d.QrCode = _fileUrlBuilderService.GetMediaUrl(d.QrCode, MediaType.qrCode));
            return data;
        }

        public async Task<ChartsResponse> chartsResponse(string gymId)
        {
            var gym = await _gymRepository.GetByIdAsync(gymId);
            if (gym == null)
                throw new NotFoundException(LocalizationKey.GymNotFound);

            return new ChartsResponse
            {
                numberOfBookings = await _gymBookingRepository.GetNumberOfBookings(gymId),
                numberOfTrainees = await _gymBookingRepository.GetNumOfTrainee(gymId),
                totalAmount = await _gymBookingRepository.GetTotalAmount(gymId),
                years = await _gymBookingRepository.GetCollectedPriceByYear(gymId),
            };
        }

        public async Task<List<UserPackageResponse>> UserPackageResponses(int id)
        {
            return await _gymBookingRepository.UserPackageResponses(id);
        }

        public async Task<QrValidationResult> ValidateQrAsync(int bookingId)
        {
            var booking = await _gymBookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                return QrValidationResult.Invalid("BookingNotFound");

            if (booking.BookingStatus == GymBookingStatus.Cancelled)
                return QrValidationResult.Invalid("BookingCancelled");

            if (booking.BookingStatus == GymBookingStatus.Completed)
                return QrValidationResult.Invalid("BookingExpired");

            var now = _dateTimeHelper.Now;
            var expiryDate = booking.Date.AddMonths(booking.Service.DurationInMonths);

            if (now > expiryDate)
                return QrValidationResult.Invalid("QrExpired");

            if (booking.IsQrUsed)
                return QrValidationResult.Invalid("QrAlreadyUsed");

            booking.IsQrUsed = true;
            booking.QrUsedAt = now;

            _gymBookingRepository.Update(booking);
            await _gymBookingRepository.SaveChangesAsync();

            return QrValidationResult.Valid();
        }
    }
}
