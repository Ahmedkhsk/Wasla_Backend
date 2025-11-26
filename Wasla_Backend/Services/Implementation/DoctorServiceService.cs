using System.Data;

namespace Wasla_Backend.Services.Implementation
{
    public class DoctorServiceService : IDoctorServiceService
    {
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IGenericRepository<ServiceDate> _serviceDateRepo;
        private readonly IGenericRepository<ServiceDay> _serviceDayRepo;
        private readonly IGenericRepository<TimeSlot> _timeSlotRepo;
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;


        public DoctorServiceService(IDoctorServiceRepository doctorServiceRepository
            ,IDoctorRepository doctorRepository
            ,IGenericRepository<ServiceDate> serviceDateRepo
            ,IGenericRepository<ServiceDay> serviceDayRepo
            , IGenericRepository<TimeSlot> timeSlotRepo
            , IUserRepository userRepository
            , IBookingRepository bookingRepository
            )
        {
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _serviceDateRepo = serviceDateRepo;
            _serviceDayRepo = serviceDayRepo;
            _timeSlotRepo = timeSlotRepo;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
        }
        public async Task AddServiceAsync(ServiceDto addServiceDto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(addServiceDto.doctorId);
            
            if (doctor == null)
            {
                throw new NotFoundException("DoctorNotFound");
            }

            var doctorService = new Service
            {
                doctorId = addServiceDto.doctorId,
                serviceName = addServiceDto.serviceName,
                description = addServiceDto.description,
                price = addServiceDto.price,
                ServiceDays = addServiceDto.serviceDays.Select(dayDto => new ServiceDay
                {
                    dayOfWeek = dayDto.dayOfWeek,
                }).ToList(),
                ServiceDates = addServiceDto.serviceDates.Select(dateDto => new ServiceDate
                {
                    date = dateDto.date,
                }).ToList(),
                TimeSlots = addServiceDto.timeSlots.Select(slotDto => new TimeSlot
                {
                    start = slotDto.start,
                    end = slotDto.end,
                }).ToList()
            };
            await _doctorServiceRepository.AddAsync(doctorService);
            await _doctorServiceRepository.SaveChangesAsync();
        }
        public async Task UpdateServiceAsync(UpdateServiceDto updateServiceDto)
        {
            var service = await _doctorServiceRepository.GetByIdAsync(updateServiceDto.serviceId);

            if (service == null)
                throw new NotFoundException("ServiceNotFound");

            service.serviceName = updateServiceDto.serviceName;
            service.description = updateServiceDto.description;
            service.price = updateServiceDto.price;

            if (updateServiceDto.serviceDays != null)
            {
                var oldDays = await _serviceDayRepo.FindAsync(sd => sd.serviceId == service.id);
                _serviceDayRepo.RemoveRange(oldDays);
                await _serviceDayRepo.SaveChangesAsync();

                service.ServiceDays = updateServiceDto.serviceDays
                    .Select(d => new ServiceDay
                    {
                        dayOfWeek = d.dayOfWeek,
                        serviceId = service.id
                    })
                    .ToList();
            }

            if (updateServiceDto.serviceDates != null)
            {
                var oldDates = await _serviceDateRepo.FindAsync(sd => sd.serviceId == service.id);
                _serviceDateRepo.RemoveRange(oldDates);
                await _serviceDateRepo.SaveChangesAsync();

                service.ServiceDates = updateServiceDto.serviceDates
                    .Select(d => new ServiceDate
                    {
                        date = d.date,
                        serviceId = service.id
                    })
                    .ToList();
            }

            var oldSlots = await _timeSlotRepo.FindAsync(t => t.serviceId == service.id);
            _timeSlotRepo.RemoveRange(oldSlots);
            await _timeSlotRepo.SaveChangesAsync();

            service.TimeSlots = updateServiceDto.timeSlots
                .Select(t => new TimeSlot
                {
                    start = t.start,
                    end = t.end,
                    serviceId = service.id
                })
                .ToList();

            _doctorServiceRepository.Update(service);
            await _doctorServiceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceResponse>> GetServices(string docotorId,string lan)
        {
            var doctor = await _doctorRepository.GetByIdAsync(docotorId);

            if (doctor == null)
                throw new NotFoundException("DoctorNotFound");
            
            var services = await _doctorServiceRepository.GetAllServicesAsync(docotorId);

            return services.Select(service => new ServiceResponse
            {
                id = service.id,
                serviceNameArabic = service.serviceName.Arabic,
                serviceNameEnglish = service.serviceName.English,
                descriptionArabic = service.description.Arabic,
                descriptionEnglish = service.description.English,
                price = service.price,
                serviceDays = service.ServiceDays?
                .Select(day => new ServiceDayResponse
                {
                    id = day.id,
                    dayOfWeek = day.dayOfWeek,
                })
                .ToList() ?? new List<ServiceDayResponse>(),
                serviceDates = service.ServiceDates?
                .Select(date => new ServiceDateResponse
                {
                    id = date.id,
                    date = date.date,
                })
                .ToList() ?? new List<ServiceDateResponse>(),
                timeSlots = service.TimeSlots.Select(slot => new TimeSoltsResponse
                {
                    id = slot.id,
                    start = slot.start,
                    end = slot.end,
                }).ToList()
            });
        
        }
        public async Task DeleteServiceAsync(int serviceId)
        {
            var service =  await _doctorServiceRepository.GetByIdAsync(serviceId);
            
            if(service == null)
            {
                throw new NotFoundException("ServiceNotFound");
            }

            await _doctorServiceRepository.DeleteByIdAsync(serviceId);
            await _doctorServiceRepository.SaveChangesAsync();
        }

        public async Task Book(BookServiceDto bookServiceDto)
        {
            var service = await _doctorServiceRepository.GetByIdAsync(bookServiceDto.ServiceId);
            if (service == null)
                throw new NotFoundException("ServiceNotFound");

            var user = await _userRepository.GetUserByIdAsync(bookServiceDto.UserId);
            if (user == null)
                throw new NotFoundException("UserNotFound");

            var serviceProvider = await _userRepository.GetUserByIdAsync(bookServiceDto.ServiceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException("ServiceProviderNotFound");

            if (service.isbooked)
                throw new BadRequestException("ServiceAlreadyBooked");


            var booking = new Booking
            {
                UserId = bookServiceDto.UserId,
                ServiceId = bookServiceDto.ServiceId,
                ServiceProviderId = bookServiceDto.ServiceProviderId,
                Price = bookServiceDto.Price,
                ServiceProviderType = bookServiceDto.ServiceProviderType,
                BookingDate = DateTime.Now
            };
            if (serviceProvider is Doctor doctor)
            {
                doctor.NumberOfPatient += 1;
                _doctorRepository.Update(doctor);
            }

            service.isbooked = true;
            _doctorServiceRepository.Update(service);
            await _bookingRepository.AddAsync(booking);

            try
            {
                await _doctorServiceRepository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("IX_Booking_ServiceId") == true)
                    throw new BadRequestException("ServiceAlreadyBooked");

                throw;
            }
        }

    }
}
