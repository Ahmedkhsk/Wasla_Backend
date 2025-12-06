namespace Wasla_Backend.Services.Implementation
{
    public class DoctorServiceService : IDoctorServiceService
    {
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IGenericRepository<ServiceDay> _serviceDayRepo;

        public DoctorServiceService(IDoctorServiceRepository doctorServiceRepository
            ,IDoctorRepository doctorRepository
            ,IGenericRepository<ServiceDay> serviceDayRepo
            )
        {
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _serviceDayRepo = serviceDayRepo;
        
        }
        public async Task AddServiceAsync(ServiceDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.doctorId);
            if (doctor == null)
                throw new NotFoundException("DoctorNotFound");

            var service = new Service
            {
                doctorId = dto.doctorId,
                serviceName = dto.serviceName,
                description = dto.description,
                price = dto.price
            };

            await _doctorServiceRepository.AddAsync(service);
            await _doctorServiceRepository.SaveChangesAsync();

            if (dto.serviceDays != null && dto.timeSlots != null)
            {
                var serviceDays = dto.serviceDays
                    .SelectMany(day => dto.timeSlots.Select(slot => new ServiceDay
                    {
                        serviceId = service.id,
                        dayOfWeek = day.dayOfWeek,
                        start = slot.start,
                        end = slot.end
                    }))
                    .ToList();

                await _serviceDayRepo.AddRangeAsync(serviceDays);
                await _serviceDayRepo.SaveChangesAsync();
            }
        }

        public async Task UpdateServiceAsync(UpdateServiceDto dto)
        {
            var service = await _doctorServiceRepository.GetServiceIncludeDaysAsync(dto.serviceId);
            if (service == null)
                throw new NotFoundException("ServiceNotFound");

            if (service.ServiceDays.Any(s => s.isBooking))
                throw new BadRequestException("CannotUpdateServiceWithExistingBookings");

            service.serviceName = dto.serviceName;
            service.description = dto.description;
            service.price = dto.price;

            var oldDays = await _serviceDayRepo.FindAsync(x => x.serviceId == service.id);
            if (oldDays.Any())
            {
                _serviceDayRepo.RemoveRange(oldDays);
                await _serviceDayRepo.SaveChangesAsync();
            }

            if (dto.serviceDays != null && dto.timeSlots != null)
            {
                var newDays = dto.serviceDays
                    .SelectMany(day => dto.timeSlots.Select(slot => new ServiceDay
                    {
                        serviceId = service.id,
                        dayOfWeek = day.dayOfWeek,
                        start = slot.start,
                        end = slot.end
                    }))
                    .ToList();

                await _serviceDayRepo.AddRangeAsync(newDays);
                await _serviceDayRepo.SaveChangesAsync();

                service.ServiceDays = newDays;
            }

            _doctorServiceRepository.Update(service);
            await _doctorServiceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceResponse>> GetServices(string doctorId, string lan)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
                throw new NotFoundException("DoctorNotFound");

            var services = await _doctorServiceRepository.GetAllServicesAsync(doctorId);

            return services.Select(service => new ServiceResponse
            {
                id = service.id,
                serviceNameArabic = service.serviceName.Arabic,
                serviceNameEnglish = service.serviceName.English,
                descriptionArabic = service.description.Arabic,
                descriptionEnglish = service.description.English,
                price = service.price,

                serviceDays = service.ServiceDays
                    .GroupBy(d => d.dayOfWeek)
                    .Select(g => new ServiceDayResponse
                    {
                        dayOfWeek = g.Key,
                        timeSlots = g.Select(slot => new SlotsResonse
                        {
                            id = slot.id,
                            start = slot.start,
                            end = slot.end,
                            isBooking = slot.isBooking
                        }).ToList()
                    })
                    .ToList()
            });
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            var service =  await _doctorServiceRepository.GetServiceIncludeDaysAsync(serviceId);
            
            if(service == null)
                throw new NotFoundException("ServiceNotFound");

            if (service.ServiceDays.Any(s => s.isBooking))
                throw new BadRequestException("CannotDeleteServiceWithExistingBookings");

            await _doctorServiceRepository.DeleteByIdAsync(serviceId);
            await _doctorServiceRepository.SaveChangesAsync();
        }
    }
}
