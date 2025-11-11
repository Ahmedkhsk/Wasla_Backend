
using System.Text.RegularExpressions;
using Wasla_Backend.DTOs.DoctorDTO;

namespace Wasla_Backend.Services.Implementation
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepository _ResidentRepository;
        private readonly IResidentIdentityRepository _ResidentIdentityRepository;

        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ResidentService> _localizer;
        private readonly string _imagePath;

        public ResidentService(IResidentRepository ResidentRepository,
            IResidentIdentityRepository ResidentIdentityRepository,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper,
            IStringLocalizer<ResidentService> localizer)
        {
            _ResidentRepository = ResidentRepository;
            _ResidentIdentityRepository = ResidentIdentityRepository;
            _mapper = mapper;
            _localizer = localizer;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathUser.TrimStart('/'));
        }
        
        public async Task CompleteResidentRegister(ResidentCompleteRegisterDto model)
        {
            var regex = new Regex(@"^\d{14}$");
            if (!regex.IsMatch(model.NationalId))
                throw new BadRequestException(_localizer["InvalidNationalId"]);

            var existingIdentity = await _ResidentIdentityRepository.GetByNationalID(model.NationalId);
            if (existingIdentity == null)
                throw new BadRequestException(_localizer["NoUnitFound"]);

            var resident = await _ResidentRepository.GetByEmail(model.Email);
            if (resident == null)
                throw new NotFoundException(_localizer["UserNotFound"]);

            _mapper.Map(model, resident);
            resident.ProfilePhoto = await FileOperation.SaveFile(model.Image, _imagePath);

            _ResidentRepository.Update(resident);
            await _ResidentRepository.SaveChangesAsync();



        }
    }
}
