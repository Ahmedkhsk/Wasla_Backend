namespace Wasla_Backend.Services.Implementation.GymService
{
    public class GymBookingService : IGymBookingService
    {
        private readonly IGymBookingRepository _gymBookingRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IGymRepository _gymRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IMapper _mapper;
      
        public GymBookingService(IGymBookingRepository gymBookingRepository, IPackageRepository packageRepository,
            IGymRepository gymRepository, IResidentRepository residentRepository,IMapper mapper)
        {
            _gymBookingRepository = gymBookingRepository;
            _packageRepository = packageRepository;
            _gymRepository = gymRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;

        }

        public async Task<BookHubData> Book(GymBookDto gymBookDto)
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

            int durationInMonths = 0;

            var gymBooking = _mapper.Map<GymBooking>(gymBookDto);

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

            await _gymBookingRepository.AddAsync(gymBooking);
            await _gymBookingRepository.SaveChangesAsync();

            var expiryDate = gymBooking.BookingDate.AddMonths(durationInMonths);
            var delay = expiryDate - DateTime.UtcNow;
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.Zero;

            BackgroundJob.Schedule<GymBookingService>(
                x => x.ExpireBooking(gymBooking.Id),
                delay
            );

            return new BookHubData
            {
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

        public Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status)
        {
           var resident = _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException("Residentnotfound");
            return _gymBookingRepository.PackagebookingOfResidentAndStatus(residentId, status);
        }
    }
}
