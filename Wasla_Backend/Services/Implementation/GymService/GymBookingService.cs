namespace Wasla_Backend.Services.Implementation.GymService
{
    public class GymBookingService : IGymBookingService
    {
        private readonly IGymBookingRepository _gymBookingRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IGymRepository _gymRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IMapper _mapper;
        private readonly string _qrPath;


        public GymBookingService(IGymBookingRepository gymBookingRepository, 
                                 IPackageRepository packageRepository,
                                 IGymRepository gymRepository,
                                 IResidentRepository residentRepository,
                                 IMapper mapper,
                                 IWebHostEnvironment webHostEnvironment, 
                                 DateTimeHelper dateTimeHelper)
        {
            _gymBookingRepository = gymBookingRepository;
            _packageRepository = packageRepository;
            _gymRepository = gymRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;
            _qrPath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.QrCodePath.TrimStart('/'));
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<BookResponse> Book(GymBookDto gymBookDto,string lan)
        {
            var gym = await _gymRepository.GetByIdAsync(gymBookDto.gymId);
            if (gym == null)
                throw new NotFoundException("Gymnotfound");

            var resident = await _residentRepository.GetByIdAsync(gymBookDto.residentId);
            if (resident == null)
                throw new NotFoundException("Residentnotfound");

            var service = await _packageRepository.GetByIdAsync(gymBookDto.serviceId);
            if (service == null)
                throw new NotFoundException("Packagenotfound");
            var IsexistingBooking = await _gymBookingRepository.IsBookingExist(gymBookDto.residentId, gymBookDto.serviceId);
            if (IsexistingBooking)
                throw new BadRequestException("PackageAlreadyBooked");

            int durationInMonths = 0;

            var gymBooking = _mapper.Map<GymBooking>(gymBookDto);

            gymBooking.BookingDate = _dateTimeHelper.Now;

            durationInMonths = service.DurationInMonths;
            gymBooking.price = service.Price;
            if (service.type == GymServiceType.Package)
            {
                durationInMonths = service.DurationInMonths;
                gymBooking.price = service.Price;
            }
            else 
            {
                var discountValue = service.Price * (service.Precentage / 100m);
                gymBooking.price = service.Price - discountValue;
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
                bookingTime = gymBooking.BookingDate,
                expiryDate = gymBooking.BookingDate.AddMonths(durationInMonths)
            };
            var qrcode=QRHelper.GenerateQRFile(QrData);
            var filePath=await FileOperation.SaveFile(qrcode, _qrPath);

            await _gymBookingRepository.AddAsync(gymBooking);
            await _gymBookingRepository.SaveChangesAsync();

            var expiryDate = gymBooking.BookingDate.AddMonths(durationInMonths);
            var delay = expiryDate - _dateTimeHelper.Now;
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.Zero;

            BackgroundJob.Schedule<GymBookingService>(
                x => x.ExpireBooking(gymBooking.Id),
                delay
            );

            return new BookResponse
            {
                qrCodeUrl = filePath,
                serviceId = gymBookDto.serviceId,
                serviceProviderId = gymBookDto.gymId,
                residentId = gymBookDto.residentId

            };
        }

        public async Task ExpireBooking(int gymBookingId)
        {
            var booking = await _gymBookingRepository.GetByIdAsync(gymBookingId);
            if (booking == null) return;

            booking.BookingStatus = GymBookingStatus.Completed;
            await _gymBookingRepository.SaveChangesAsync();
        }

        public async Task<BookHubData> Cancel(int bookingId)
        {
            var booking =await _gymBookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new NotFoundException("Bookingnotfound");
            booking.BookingStatus = GymBookingStatus.Cancelled;
            _gymBookingRepository.Update(booking);
            await _gymBookingRepository.SaveChangesAsync();
            var bookHubData = new BookHubData
            {
                serviceId = booking.ServiceId,
                serviceProviderId = booking.GymId,
                residentId = booking.ResidentId
            };
            return bookHubData;
        }

        public async Task<List<BookingOfGym>> PackageBookingOFGym(string gymId)
        {
            var gym = await _gymRepository.GetByIdAsync(gymId);
            if(gym==null)
                throw new NotFoundException("Gymnotfound");
            return await _gymBookingRepository.PackagebookingOfGym(gymId);

        }

        public async Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status)
        {
            var gym = await _gymRepository.GetByIdAsync(gymId);
            if (gym == null)
                throw new NotFoundException("Gymnotfound");
            return await _gymBookingRepository.PackagebookingOfGymAndStatus(gymId,status);
        }

        public async Task<List<BookingOfUser>> PackagebookingOfResident(string residentId)
        {
           var resident =await _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException("Residentnotfound");
            return await _gymBookingRepository.PackagebookingOfResident(residentId);
        }

        public async Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status)
        {
           var resident = await _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException("Residentnotfound");
            return await _gymBookingRepository.PackagebookingOfResidentAndStatus(residentId, status);
        }

        public async Task<ChartsResponse> chartsResponse(string gymId)
        {
            var gym = await _gymRepository.GetByIdAsync(gymId);
            
            if (gym == null)
                throw new NotFoundException("Gymnotfound");
            return new ChartsResponse
            {
                numberOfBookings = await _gymBookingRepository.GetNumberOfBookings(gymId),
                numberOfTrainees = await _gymBookingRepository.GetNumOfTrainee(gymId),
                totalAmount = await _gymBookingRepository.GetTotalAmount(gymId),
                years = await _gymBookingRepository.GetCollectedPriceByYear(gymId),
            };
        }

        public async Task<List<UserPackageResponse>> UserPackageResponses(GymServiceType type)
        {
            return await _gymBookingRepository.UserPackageResponses(type);
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
            var expiryDate = booking.BookingDate.AddMonths(booking.Service.DurationInMonths);

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
