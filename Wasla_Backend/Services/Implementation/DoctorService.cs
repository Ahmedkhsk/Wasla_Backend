
using Microsoft.AspNetCore.Mvc.Razor;

namespace Wasla_Backend.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<DoctorSpecialization> _doctorSpecializationRepository;
        private readonly string _imagePath;
        private readonly string _cvPath;


        public DoctorService(IDoctorRepository doctorRepository, 
            IWebHostEnvironment webHostEnvironment, 
            IMapper mapper, 
            IStringLocalizer<DoctorService> localizer,
            IGenericRepository<DoctorSpecialization> doctorSpecializationRepository)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathUser.TrimStart('/'));
            _cvPath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.PathCVDoctor.TrimStart('/'));
        }
        public async Task CompleteData(DoctorCompleteDto doctorCompleteDto)
        {
            var doctor = await _doctorRepository.GetByEmail(doctorCompleteDto.Email);
            
            if(doctor == null)
                throw new NotFoundException("UserNotFound");

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

            
            var SpecializationResponse = doctorSpecialization.Select(ds => new DoctorSpecializationResponse
            {
                Id = ds.Id,
                Name = ds.Specialization.GetText(lan)
            });

            return SpecializationResponse;
        }
        public async Task<DoctorProfileResponse> GetDoctorProfile(string id , string lan)
        {
            var doctor = await _doctorRepository.GetById(id);
            
            if (doctor == null)
                throw new NotFoundException("DoctorNotFound");

            var doctorProfileResponses = _mapper.Map<DoctorProfileResponse>(doctor, opt =>
            {
                opt.Items["lan"] = lan;
            });

            return doctorProfileResponses;
        }
    }
}
