namespace Wasla_Backend.Services.Implementation
{
    public class BookService: IBookService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ServiceDay> _serviceDayRepository;
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly  IResidentRepository _residentRepository;
        private readonly string _imagePath;
        public BookService( IBookingRepository bookingRepository, 
                            IUserRepository userRepository, 
                            IGenericRepository<ServiceDay> serviceDay,
                            IWebHostEnvironment webHostEnvironment, 
                            IDoctorServiceRepository doctorServiceRepository,
                            IDoctorRepository doctorRepository,
                            IResidentRepository residentRepository

            )
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _serviceDayRepository = serviceDay;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathBooking.TrimStart('/'));
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _residentRepository = residentRepository;
        }
        public async Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("UserNotFound");
            }
            return await _bookingRepository.GetBookingDetailsForUserAsync(userId, language);
        }

        public async Task Book(BookServiceDto dto)
        {

            var user = await _residentRepository.GetByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException("UserNotFound");

            var serviceProvider = await _userRepository.GetUserByIdAsync(dto.serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException("ServiceProviderNotFound");

            var serviceDay = await _serviceDayRepository.GetByIdAsync(dto.serviceDayId);
            if (serviceDay == null)
                throw new NotFoundException("ServiceDayNotFound");

            if (serviceDay.isBooking)
                throw new BadRequestException("ServiceAlreadyBooked");


            List<string> savedImages = new();
            if (dto.images != null)
            {
                foreach (var img in dto.images)
                {
                    var path = await FileOperation.SaveFile(img, _imagePath);
                    savedImages.Add(path);
                }
            }
            var Booking = await _bookingRepository.GetByServiceDayId(dto.serviceDayId);
            if (Booking != null)
            {
                Booking.images = savedImages;
                Booking.bookingDate = dto.bookingDate;
                Booking.bookingStatus = BookingStatus.upcoming;
                Booking.userId = dto.userId;
                serviceDay.isBooking = true;
                _bookingRepository.Update(Booking);

            }
            else
            {
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
            }
            if (serviceProvider is Doctor doc)
            {
                var IsExistingBooking = await _bookingRepository.GetByUserIdAndDoctorID(dto.userId, doc.Id);
                if (!IsExistingBooking)
                {
                    doc.numberOfpatients += 1;
                    _doctorRepository.Update(doc);
                }
            }
            try
            {
                await _bookingRepository.SaveChangesAsync();
                await _doctorRepository.SaveChangesAsync(); 

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Booking_serviceDayId") == true)
                    throw new BadRequestException("ServiceAlreadyBooked");

                throw;                                        
            }
        }
    }
}
