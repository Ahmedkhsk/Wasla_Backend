namespace Wasla_Backend.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DoctorSpecialization> _doctorSpecializationRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IMapper mapper,
            IStringLocalizer<DoctorService> localizer,
            IGenericRepository<DoctorSpecialization> doctorSpecializationRepository,
            IBookingRepository bookingRepository,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _bookingRepository = bookingRepository;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task CompleteData(DoctorCompleteDto doctorCompleteDto)
        {
            var doctor = await _doctorRepository.GetByEmail(doctorCompleteDto.Email);
            if (doctor == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            _mapper.Map(doctorCompleteDto, doctor);

            doctor.ProfilePhoto = await _fileService.AddFileAsync(
                doctorCompleteDto.Image,
                _fileUrlBuilderService.GetPath(MediaType.userImage)
            );
            doctor.CV = await _fileService.AddFileAsync(
                doctorCompleteDto.CV,
                _fileUrlBuilderService.GetPath(MediaType.doctorCV)
            );
            doctor.IsCompleteRegistration = true;

            _doctorRepository.Update(doctor);
            await _doctorRepository.SaveChangesAsync();
            doctor.ProfilePhoto = _fileUrlBuilderService.GetMediaUrl(doctor.ProfilePhoto, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                doctor.Id,
                NotificationType.doctorCompleteInfoScreen,
                doctor.Id,
                doctor.ProfilePhoto,
                "en",
                null
            ));
        }

        public async Task<IEnumerable<DoctorSpecializationResponse>> DoctorSpecializations(string lan)
        {
            var doctorSpecialization = await _doctorSpecializationRepository.GetAllAsync();

            return doctorSpecialization.Select(ds => new DoctorSpecializationResponse
            {
                Id = ds.Id,
                Name = ds.Specialization.GetText(lan)
            });
        }

        public async Task<IEnumerable<AllDoctorDataDto>> GetAllDoctors(string lan)
        {
            var doctors = await _doctorRepository.GetAllSortedByRating();
            var allDoctorDataDtos = _mapper.Map<IEnumerable<AllDoctorDataDto>>(doctors);

            foreach (var doctor in allDoctorDataDtos)
                doctor.specialtyName = await _doctorRepository.GetDoctorSpecializationName(doctor.Id, lan);

            return allDoctorDataDtos;
        }

        public async Task<IEnumerable<AllDoctorDataDto>> GetDoctorBySpecialist(int specialistId, string lan)
        {
            var doctors = await _doctorRepository.GetBySpecialist(specialistId);
            var allDoctorDataDtos = _mapper.Map<IEnumerable<AllDoctorDataDto>>(doctors);

            foreach (var doctor in allDoctorDataDtos)
                doctor.specialtyName = await _doctorRepository.GetDoctorSpecializationName(doctor.Id, lan);

            return allDoctorDataDtos;
        }

        public async Task<DoctorChartDto> GetDoctorChart(string doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
                throw new NotFoundException(LocalizationKey.DoctorNotFound);

            return new DoctorChartDto
            {
                numOfPatients = await _bookingRepository.GetNumberOfPatientByDoctorId(doctorId),
                numOfBookings = await _bookingRepository.CountBookings(doctorId),
                numOfCompletedBookings = await _bookingRepository.CountCompletedBookings(doctorId),
                totalAmount = await _bookingRepository.GetTotalAmount(doctorId),
                years = await _bookingRepository.GetCollectedPriceByYear(doctorId),
            };
        }

        public async Task<List<GetAllBookingResponse>> GetAllBookingOfDoctors(string docId, BookingStatus status, string lan)
        {
            var doctor = await _doctorRepository.GetByIdAsync(docId);
            if (doctor == null)
                throw new BadRequestException(LocalizationKey.DoctorNotFound);

            if (!Enum.IsDefined(typeof(BookingStatus), status))
                throw new BadRequestException(LocalizationKey.InvalidBookingStatus);

            return await _bookingRepository.GetBookingsByDoctorIdAsync(docId, status, lan);
        }

        public async Task<DoctorProfileResponse> GetDoctorProfile(string id, string lan)
        {
            return await _doctorRepository.GetDoctorProfileById(id);
        }

        public async Task UpdateDoctorProfile(UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(updateDoctorDto.userId);
            if (doctor == null)
                throw new NotFoundException(LocalizationKey.DoctorNotFound);

            var currentImage = doctor.ProfilePhoto;
            var currentCv = doctor.CV;
            var specializationId = doctor.SpecializationId;

            _mapper.Map(updateDoctorDto, doctor);

            if (updateDoctorDto.specializationId != 0)
            {
                var specialization = await _doctorSpecializationRepository.GetByIdAsync(updateDoctorDto.specializationId);
                if (specialization == null)
                    throw new NotFoundException(LocalizationKey.SpecializationNotFound);

                doctor.SpecializationId = updateDoctorDto.specializationId;
            }
            else
            {
                doctor.SpecializationId = specializationId;
            }

            doctor.ProfilePhoto = await _fileService.ReplaceFileAsync(
                currentImage,
                updateDoctorDto.profilePhoto,
                _fileUrlBuilderService.GetPath(MediaType.userImage)
            );
            doctor.CV = await _fileService.ReplaceFileAsync(
                currentCv,
                updateDoctorDto.cv,
                _fileUrlBuilderService.GetPath(MediaType.doctorCV)
            );

            _doctorRepository.Update(doctor);
            await _doctorRepository.SaveChangesAsync();
        }

        public async Task<AllDoctorDataDto> GetDoctorData(string doctorId, string lan)
        {
            var doc = await _doctorRepository.GetByIdAsync(doctorId);
            if (doc == null)
                throw new NotFoundException(LocalizationKey.DoctorNotFound);

            var doctor = await _doctorRepository.GetDoctorData(doctorId);
            if (doctor == null)
                return null;

            doctor.specialtyName = await _doctorRepository.GetDoctorSpecializationName(doctorId, lan);

            return doctor;
        }
    }
}