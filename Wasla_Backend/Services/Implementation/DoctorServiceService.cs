namespace Wasla_Backend.Services.Implementation
{
    public class DoctorServiceService : IDoctorServiceService
    {
        private readonly IDoctorServiceRepository _doctorServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IGenericRepository<ServiceDate> _serviceDateRepo;
        private readonly IGenericRepository<ServiceDay> _serviceDayRepo;
        private readonly IGenericRepository<TimeSlot> _timeSlotRepo;

        public DoctorServiceService(IDoctorServiceRepository doctorServiceRepository
            ,IDoctorRepository doctorRepository
            ,IGenericRepository<ServiceDate> serviceDateRepo
            ,IGenericRepository<ServiceDay> serviceDayRepo
            , IGenericRepository<TimeSlot> timeSlotRepo)
        {
            _doctorServiceRepository = doctorServiceRepository;
            _doctorRepository = doctorRepository;
            _serviceDateRepo = serviceDateRepo;
            _serviceDayRepo = serviceDayRepo;
            _timeSlotRepo = timeSlotRepo;
        }
        public async Task AddServiceAsync(AddServiceDto addServiceDto)
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

            _serviceDayRepo.RemoveRange(service.ServiceDays);
            _serviceDateRepo.RemoveRange(service.ServiceDates);
            _timeSlotRepo.RemoveRange(service.TimeSlots);

            service.ServiceDays = updateServiceDto.serviceDays
                .Select(d => new ServiceDay { dayOfWeek = d.dayOfWeek })
                .ToList();

            service.ServiceDates = updateServiceDto.serviceDates
                .Select(d => new ServiceDate { date = d.date })
                .ToList();

            service.TimeSlots = updateServiceDto.timeSlots
                .Select(t => new TimeSlot { start = t.start, end = t.end })
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
                serviceName = service.serviceName.GetText(lan),
                description = service.description.GetText(lan),
                price = service.price,
                serviceDays = service.ServiceDays.Select(day => new ServiceDayResponse
                {
                    id = day.id,
                    dayOfWeek = day.dayOfWeek,
                }).ToList(),
                serviceDates = service.ServiceDates.Select(date => new ServiceDateResponse
                {
                    id = date.id,
                    date = date.date,
                }).ToList(),
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

            _doctorServiceRepository.Delete(service);
        }

    }
}
