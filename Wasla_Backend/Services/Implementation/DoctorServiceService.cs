using Wasla_Backend.Models;

namespace Wasla_Backend.Services.Implementation
{
    public class DoctorServiceService : IDoctorServiceService
    {
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IGenericRepository<ServiceDay> _serviceDayRepo;
        private readonly IBookingRepository _bookingRepository;
        private readonly IHubContext<ServiceHub> _hub;
        public DoctorServiceService(IDoctorServiceRepository doctorServiceRepository
            ,IDoctorRepository doctorRepository
            ,IGenericRepository<ServiceDay> serviceDayRepo
            ,IBookingRepository bookingRepository
            , IHubContext<ServiceHub> hub
            )
        {
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _bookingRepository = bookingRepository;
            _serviceDayRepo = serviceDayRepo;
            _hub = hub;

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

            var serviceHubData = new ServiceHubData
            {
                serviceProviderId = dto.doctorId,
                serviceId = service.id
            };
            await _hub.Clients.All.SendAsync("ServiceAdded", serviceHubData);
        }

        public async Task UpdateServiceAsync(UpdateServiceDto dto)
        {
            var service = await _doctorServiceRepository.GetServiceIncludeDaysAsync(dto.serviceId);
            if (service == null)
                throw new NotFoundException("ServiceNotFound");

            var hasAnyBookings = await _bookingRepository
                            .AnyAsync(b => b.serviceDay.serviceId == dto.serviceId);

            if (hasAnyBookings)
                throw new BadRequestException("ServiceHasBookings");

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
            var serviceHubData = new ServiceHubData
            {
                serviceProviderId = service.doctorId,
                serviceId = service.id
            };
            await _hub.Clients.All.SendAsync("ServiceUpdated", serviceHubData);
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

            var hasAnyBookings = await _bookingRepository
                                    .AnyAsync(b => b.serviceDay.serviceId == serviceId);

            if (hasAnyBookings)
                throw new BadRequestException("ServiceHasBookings");

            if (service.ServiceDays.Any(s => s.isBooking))
                throw new BadRequestException("CannotDeleteServiceWithExistingBookings");

            await _doctorServiceRepository.DeleteByIdAsync(serviceId);
            await _doctorServiceRepository.SaveChangesAsync();
            var serviceHubData = new ServiceHubData
            {
                serviceProviderId = service.doctorId,
                serviceId = service.id
            };
            await _hub.Clients.All.SendAsync("ServiceDeleted", serviceHubData);
        }
    
    }
}
