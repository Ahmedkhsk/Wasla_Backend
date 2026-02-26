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
        private readonly string _imagePath;
        private readonly IHubContext<BookingHub> _hub;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorBookService> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<DoctorBookService>();
        private static readonly SemaphoreSlim _bookingLock = new SemaphoreSlim(1, 1);


        public DoctorBookService(IBookingRepository bookingRepository,
                            IUserRepository userRepository,
                            IGenericRepository<ServiceDay> serviceDay,
                            IWebHostEnvironment webHostEnvironment,
                            IDoctorServiceRepository doctorServiceRepository,
                            IDoctorRepository doctorRepository,
                            IResidentRepository residentRepository,
                            IHubContext<BookingHub> hub,
                            IMapper mapper

            )
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _serviceDayRepository = serviceDay;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathBooking.TrimStart('/'));
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _residentRepository = residentRepository;
            _hub = hub;
            _mapper = mapper;
        }

        public async Task UpdateBookingStatus(int bookingId, BookingStatus status)
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
                var countOfBookings =
                        await _bookingRepository.CountBookingBYUserAndServiceProvider(booking.userId, booking.serviceProviderId);
                if (countOfBookings == 1 && booking.serviceProviderType == ServiceProviderType.Doctor)
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
                residentId = booking.userId,
                serviceProviderId = booking.serviceProviderId
            };
            await _hub.Clients.All.SendAsync("Bookingcanceled", bookhubdata);
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
            {
                throw new BadRequestException(LocalizationKey.InvalidBookingUpdateDetails);
            }

            booking = _mapper.Map(updateBookingDto, booking);

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();
            var bookhubdata = new BookHubData
            {
                serviceId = booking.serviceDayId,
                residentId = booking.userId,
                serviceProviderId = booking.serviceProviderId
            };
            await _hub.Clients.All.SendAsync("BookingUpdated", bookhubdata);
        }

        public async Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(LocalizationKey.UserNotFound);
            }
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

                List<string> savedImages = new();
                if (dto.images != null)
                {
                    foreach (var img in dto.images)
                    {
                        var path = await FileOperation.SaveFile(img, _imagePath);
                        savedImages.Add(path);
                    }
                }

                var booking = new Booking
                {
                    userId = dto.userId,
                    serviceProviderId = dto.serviceProviderId,
                    price = dto.price,
                    serviceProviderType = dto.serviceProviderType,
                    bookingDate = dto.bookingDate,
                    bookingType = dto.bookingType,
                    images = savedImages,
                    serviceDayId = dto.serviceDayId
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
                return booking.Id;
            }
            finally
            {
                _bookingLock.Release();
                
            }
            
        }
    }
}