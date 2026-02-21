namespace Wasla_Backend.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DoctorSpecialization> _doctorSpecializationRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly string _imagePath;
        private readonly string _cvPath;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper,
            IStringLocalizer<DoctorService> localizer,
            IGenericRepository<DoctorSpecialization> doctorSpecializationRepository,
            IBookingRepository bookingRepository
        )
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _bookingRepository = bookingRepository;

            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathUser.TrimStart('/'));
            _cvPath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.PathCVDoctor.TrimStart('/'));
        }

        public async Task CompleteData(DoctorCompleteDto doctorCompleteDto)
        {
            var doctor = await _doctorRepository.GetByEmail(doctorCompleteDto.Email);

            if (doctor == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            _mapper.Map(doctorCompleteDto, doctor);

            var image = await FileOperation.SaveFile(doctorCompleteDto.Image, _imagePath);
            var cv = await FileOperation.SaveFile(doctorCompleteDto.CV, _cvPath);

            doctor.ProfilePhoto = image;
            doctor.CV = cv;
            doctor.IsCompleteRegistration = true;

            _doctorRepository.Update(doctor);
            await _doctorRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoctorSpecializationResponse>> DoctorSpecializations(string lan)
        {
            var doctorSpecialization = await _doctorSpecializationRepository.GetAllAsync();

            var specializationResponse = doctorSpecialization.Select(ds => new DoctorSpecializationResponse
            {
                Id = ds.Id,
                Name = ds.Specialization.GetText(lan)
            });

            return specializationResponse;
        }

        public async Task<IEnumerable<AllDoctorDataDto>> GetAllDoctors(string lan)
        {
            var doctors = await _doctorRepository.GetAllSortedByRating();

            var allDoctorDataDtos = _mapper.Map<IEnumerable<AllDoctorDataDto>>(doctors);
            foreach (var doctor in allDoctorDataDtos)
            {
                doctor.specialtyName = await _doctorRepository.GetDoctorSpecializationName(doctor.Id, lan);
            }

            return allDoctorDataDtos;
        }

        public async Task<IEnumerable<AllDoctorDataDto>> GetDoctorBySpecialist(int specialistId, string lan)
        {
            var doctors = await _doctorRepository.GetBySpecialist(specialistId);

            var allDoctorDataDtos = _mapper.Map<IEnumerable<AllDoctorDataDto>>(doctors);
            foreach (var doctor in allDoctorDataDtos)
            {
                doctor.specialtyName = await _doctorRepository.GetDoctorSpecializationName(doctor.Id, lan);
            }

            return allDoctorDataDtos;
        }

        public async Task<DoctorChartDto> GetDoctorChart(string doctorId)
        {
            var doctor = await _doctorRepository.GetById(doctorId);

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
            var doctor = await _doctorRepository.GetById(docId);

            if (doctor == null)
                throw new BadRequestException(LocalizationKey.DoctorNotFound);

            if (!Enum.IsDefined(typeof(BookingStatus), status))
                throw new BadRequestException(LocalizationKey.InvalidBookingStatus);

            return await _bookingRepository.GetBookingsByDoctorIdAsync(docId, status, lan);
        }

        public async Task<DoctorProfileResponse> GetDoctorProfile(string id, string lan)
        {
            var doctor = await _doctorRepository.GetById(id);

            if (doctor == null)
                throw new NotFoundException(LocalizationKey.DoctorNotFound);

            var doctorProfileResponse = _mapper.Map<DoctorProfileResponse>(doctor, opt =>
            {
                opt.Items["lan"] = lan;
            });

            doctorProfileResponse.numberOfpatients = await _bookingRepository.GetNumberOfPatientByDoctorId(doctor.Id);

            return doctorProfileResponse;
        }

        public async Task UpdateDoctorProfile(UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _doctorRepository.GetById(updateDoctorDto.userId);

            if (doctor == null)
                throw new NotFoundException(LocalizationKey.DoctorNotFound);

            var image = doctor.ProfilePhoto;
            var cv = doctor.CV;
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

            if (updateDoctorDto.profilePhoto != null)
            {
                FileOperation.DeleteFile(doctor.ProfilePhoto, _imagePath);
                image = await FileOperation.SaveFile(updateDoctorDto.profilePhoto, _imagePath);
                doctor.ProfilePhoto = image;
            }
            else
            {
                doctor.ProfilePhoto = image;
            }

            if (updateDoctorDto.cv != null)
            {
                FileOperation.DeleteFile(doctor.CV, _cvPath);
                cv = await FileOperation.SaveFile(updateDoctorDto.cv, _cvPath);
                doctor.CV = cv;
            }
            else
            {
                doctor.CV = cv;
            }

            _doctorRepository.Update(doctor);
            await _doctorRepository.SaveChangesAsync();
        }

        public async Task<AllDoctorDataDto> GetDoctorData(string doctorId, string lan)
        {
            var doc = await _doctorRepository.GetById(doctorId);
            if (doc == null)
                throw new NotFoundException(LocalizationKey.DoctorNotFound);

            var doctor = await _doctorRepository.GetDoctorData(doctorId);

            if (doctor == null)
                return null;

            doctor.specialtyName =
                await _doctorRepository.GetDoctorSpecializationName(doctorId, lan);

            return doctor;
        }
    }
}