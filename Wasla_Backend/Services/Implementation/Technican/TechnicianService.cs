

using Wasla_Backend.Helpers.Extensions;
using Wasla_Backend.Models.technician;

namespace Wasla_Backend.Services.Implementation.technican
{
    public class TechnicianService : ITechnicianService
    {
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IUserAuthorizationService _userAuthorizationService;
        public TechnicianService(ITechnicianRepository technicianRepository,IMapper mapper,IFileService fileService
            ,IFileUrlBuilderService fileUrlBuilderService,
            IUserAuthorizationService userAuthorizationService
            )
        {
            _technicianRepository = technicianRepository;
            _mapper = mapper;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task CompleteRegisterAsync(TechnicianCompleteRegisterDto technicianCompleteRegisterDto)
        {
            var Technician=await _technicianRepository.GetByEmailAsync(technicianCompleteRegisterDto.Email);
            if (Technician == null)
                throw new NotFoundException(LocalizationKey.TechnicianNotFound);
    
            if (technicianCompleteRegisterDto.Documents == null || technicianCompleteRegisterDto.Documents.Count == 0)
                throw new BadRequestException(LocalizationKey.DocumentsAreRequired);
            _mapper.Map(technicianCompleteRegisterDto, Technician);
            if (technicianCompleteRegisterDto.Photo != null)
                Technician.ProfilePhoto = await _fileService.AddFileAsync(
                    technicianCompleteRegisterDto.Photo,
                    _fileUrlBuilderService.GetPath(MediaType.userImage)
                );
            Technician.Documents = await _fileService.AddFilesAsync(
                technicianCompleteRegisterDto.Documents,
                _fileUrlBuilderService.GetPath(MediaType.TechnicianDocument)
            );
            Technician.IsCompleteRegistration = true;
            await _technicianRepository.SaveChangesAsync();
            var image = _fileUrlBuilderService.GetMediaUrl(Technician.ProfilePhoto, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    Technician.Id,
    NotificationType.technicianCompleteInfoScreen,
    Technician.Id,
    image,
    "en",
    null
));
        }

        public async Task<TechnicianChartDto> GetChartById(string TechnicianId)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(TechnicianId);
            var IdExist = await _technicianRepository.IsExistById(TechnicianId);
            if (!IdExist)
                throw new NotFoundException(LocalizationKey.TechnicianNotFound);
           return await _technicianRepository.GetChartById(TechnicianId);
            
        }

        public async Task<TechnicianProfileDto> GetProfileById(string id)
        {
           var profile =await _technicianRepository.GetProfileById(id);
            if (profile == null)
                throw new NotFoundException(LocalizationKey.TechnicianNotFound);
            profile.ProfilePhotoUrl = _fileUrlBuilderService.GetMediaUrl(profile.ProfilePhotoUrl, MediaType.userImage);
            if (profile.DocumentsUrls != null && profile.DocumentsUrls.Count > 0)
            {
                profile.DocumentsUrls = profile.DocumentsUrls.Select(d => _fileUrlBuilderService.GetMediaUrl(d, MediaType.TechnicianDocument)).ToList();
            }

            return profile;
        }

        public List<TechnicianSpecializationDto> GetSpecializations(string lan)
        {
            return Enum.GetValues(typeof(TechnicianSpecialty))
                .Cast<TechnicianSpecialty>()
                .Select(s => new TechnicianSpecializationDto {
                    Id = (int)s,
                    Name = s.GetName(lan)
                }).ToList();
        }

        public async Task<PagedResult<TechnicianListDto>> GetTechniciansBySpecialty(
       TechnicianSpecialty? specialty, int pageNumber, int pageSize, string lan)
        {
            var technicians = await _technicianRepository.GetTechniciansBySpecialty(specialty, pageNumber, pageSize, lan);

            technicians.ForEach(t =>
            {
                t.ImageUrl = _fileUrlBuilderService.GetMediaUrl(t.ImageUrl, MediaType.userImage);
            });

            return new PagedResult<TechnicianListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = technicians.Count, 
                Data = technicians
            };
        }

        public async Task UpdateProfile(TechnicianUpdateProfileDto technicianUpdateProfileDto)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(technicianUpdateProfileDto.Id);
            var Technician =await _technicianRepository.GetByIdAsync(technicianUpdateProfileDto.Id);
            if (Technician == null)
                throw new NotFoundException(LocalizationKey.TechnicianNotFound);
            _mapper.Map(technicianUpdateProfileDto, Technician);
            if(technicianUpdateProfileDto.ProfilePhoto!=null)
            { 
                
                
                Technician.ProfilePhoto = await _fileService.ReplaceFileAsync(
                    Technician.ProfilePhoto,
                    technicianUpdateProfileDto.ProfilePhoto,
                    _fileUrlBuilderService.GetPath(MediaType.userImage)
                );
            }


            if(technicianUpdateProfileDto.Documents != null && technicianUpdateProfileDto.Documents.Count > 0)
            {
                _fileService.DeleteFiles(Technician.Documents,_fileUrlBuilderService.GetPath(MediaType.TechnicianDocument));
                Technician.Documents = await _fileService.AddFilesAsync(
                    technicianUpdateProfileDto.Documents,
                    _fileUrlBuilderService.GetPath(MediaType.TechnicianDocument)
                );
            }

               await _technicianRepository.SaveChangesAsync();
        }
    }
}
